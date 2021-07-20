<template lang="html">
  <div>
    <backdrop :isShow="showWaiting" background="rgba(7,17,27,0.6)"></backdrop>
    <div class="spin" v-show="showWaiting">
      <spin size="large" v-show="showWaiting"></spin>
    </div>
  <takeawayTimeSlots ref="takeawayTimeSlots" :takeawayTimeSlots="takeawayTimeSlots" @timeSlotSelected="timeSlotSelected"></takeawayTimeSlots>
  <transition name="fade">
    <div v-show="show" class="detail" @click="hidecheckout()">
    <!-- <div class="detail" @click="showToggle()"> -->
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="show">
      <div class="billcontainer">
          <div class="columnpadding"></div>
          <div class="billcontent">
            <ul>
              <li class="rowheader">
                <span class="columncount">{{ml.ordercount}}</span>
                <span class="columnname">{{ml.orderdescription}}</span>
                <span class="columnunitprice">{{ml.orderunitprice}}</span>
                <span class="columnprice">{{ml.orderlinetotal}}</span>
              </li>
              <li class="row" v-for="food in selectFoods">
                <span class="columncount">{{food.count}}</span>
                <span class="columnname">{{food.name[data.currentlnindex]}}</span>
                <span class="columnunitprice">{{food.price}} </span>
                <span class="columnprice">{{(food.price * food.count).toFixed(2)}} </span>
              </li>
              <li class="footline">
                <span class="columncount">{{totalCount}}</span>
                <span class="columnname"></span>
                <span class="columnunitprice"></span>
                <span class="columnprice">{{totalPrice}}</span>
              </li>
            </ul>
            <div class="totalline">{{ml.ordertotal}}        €{{totalPrice}}</div>
            <div class="timeSlotWrapper" @click="showTakeawayTimeSlots" v-if="this.data.options.takeaway === '1'">
              <div class = "timeSlotContainer">
              {{ml.takeawaytime}}: <span>{{takeawayTimeSlot}}</span>
              <Icon type="ios-time-outline" />
              </div>
            </div>
            <div class="openinghourcomment" v-if="openinghourComment !== ''">{{openinghourComment}}</div>
            <div class="ordercomment" v-if="seller.supportOnlineOrder">
              <textarea class="ordercommenttext" v-model="ordercomment" rows="2" cols="100%" :placeholder="ml.ordercomment" maxlength="100"></textarea>
            </div>
            <div class="ordertext" v-if="!seller.supportOnlineOrder">
              {{ml.orderonlineordernotsupported}} {{seller.telefoon[0]}}
            </div>
            <div class="buttonarea">
              <sendButton ref="mySendButton" :timeout="15" :text="ml.confirm" :ml="ml" :sendingText="ml.sending" :failedText="ml.ordersendfailed + ' ' + seller.telefoon" @click="sendOrder"></sendButton>
              <i-button type="primary" @click="hidecheckout">{{ml.cancel}}</i-button>
            </div>
          </div>
          <div class="columnpadding"></div>
          <!-- <div class="button">

          </div> -->
      </div>

    </div>
  </transition>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'
import axios from 'axios'
import backdrop from 'components/backdrop/backdrop'
import takeawayTimeSlots from 'components/timeSlots/timeSlots'
import sendButton from 'components/common/sendButton/sendButton'

export default {
  components: {
    backdrop,
    takeawayTimeSlots,
    sendButton
  },
  props: {
    seller: {},
    selectFoods: {
      type: Array,
      default: []
    },
    totalPrice: 0,
    ml: {},
    data: {}
  },
  data() {
    return {
      show: false,
      // trans: ml.trans, /* ml without this. it search from global js. in this case from data.js */
      userName: 'vue app',
      simpleHubProxy: null,
      connectionId: '',
      ordercomment: '',
      showWaiting: false,
      mySendingTimer: {},
      startTime: {},
      // takeawayTimeSlot: '12:00', /* give some value, otherwise order string will not correct and crash server */
      takeawayTimeSlot: this.ml.selecttime, /* give some value, otherwise order string will not correct and crash server */
      takeawayTimeSlots: [],
      busyWithSending: false,
      shopClosed: false,
      openinghourComment: '',
      takeawayTimeSlotsGot: false
    }
  },
  computed: {
    totalCount() {
      let count = 0
      this.selectFoods.forEach((food) => {
        count += food.count
      })
      return count
    },
    orderRequestString() {
      let orderline = ''
      let requestString = ''
      for (var icount = 0; icount < this.selectFoods.length; icount++) {
        if (icount > 0) {
          orderline = orderline + '@@'
        }
        orderline = orderline + this.selectFoods[icount].menunr + '@' + this.selectFoods[icount].count
      }
      requestString = 'id:b613762f-29b9-442d-871c-9c9344ff6e4c@@@ORDER@@@20090808001@@@' /* template for RestSoft.WPF */
      requestString = requestString + 'LastName@@FirstName@@Address@@Postcode@@Place@@Telephone@@GSM@@Password@@Title@@Email'
      requestString = requestString + '@@@Option1@@Today@@' + this.takeawayTimeSlot + '@@@'
      requestString = requestString + orderline + '@@@' + this.ordercomment + '@@@' + JSON.stringify(this.data.options)
      console.log(this.data.options)
      return requestString
    },
    shouldGetTakeawayTimeSlots () {
      return this.data.options.takeaway === '1' && this.selectFoods.length > 0
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    // this.connectToSignalRServer()
    this.$root.eventHub.$on('signalr.onOrderConfirmedFromServerToWeb', this.onOrderConfirmedFromServerToWeb)
    this.$root.eventHub.$on('signalr.onGetTakeawayTimeSlots', this.onGetTakeawayTimeSlots)
    this.$root.eventHub.$on('signalr.onSessionExpired', this.onSessionExpired)

    console.log('checkout JSON.stringify(this.trans)')
    console.log(JSON.stringify(this.trans))
  },
  watch: {
    show(newValue, oldValue) {
      console.log('checkout show')
      console.log(newValue)
      if (newValue && this.shouldGetTakeawayTimeSlots) {
        this.takeawayTimeSlotsGot = false
        this.$root.eventHub.$emit('signalr.getTakeawayTimeSlots', this.orderRequestString)
      } else {
        this.hidecheckout()
      }
    }
  },
  methods: {
    _initScroll() {
      let smallScreen = screen.width <= 800;
      console.log(`screen width ${screen.width}, smallScreen ${smallScreen}`)
      this.foodsScroll = new BScroll(this.$refs.detailWrapper, {
        click: true,
        probeType: 3,
        scrollbar: {
          fade: smallScreen,
          interactive: !smallScreen // new in 1.8.0
        },
        mouseWheel: {
          speed: 20,
          invert: false
        }
      });
    },
    showcheckout() {
      this.show = true;
      this.$nextTick(() => {
        this.foodsScroll.refresh(); // 初始化scroll
      })
    },
    hidecheckout() {
      this.show = false;
    },
    showTakeawayTimeSlots() {
      if (this.shouldGetTakeawayTimeSlots && !this.takeawayTimeSlotsGot) {
        this.$Modal.info({
          content: this.ml.cannotconnecttorestaurantfortimeslots,
          okText: this.ml.ok
        });
      } else if (this.allowPlaceOrder) {
        this.$refs.takeawayTimeSlots.showTimeSlots()
      }
    },
    onGetTakeawayTimeSlots(slots) {
      this.takeawayTimeSlotsGot = true
      // console.log(slots)
      let mySlots = slots // JSON.parse(slots)
      // console.log(mySlots)
      // console.log(typeof mySlots.timeSlots)
      this.takeawayTimeSlots = mySlots.timeSlots
      this.openinghourComment = mySlots.message
      this.allowPlaceOrder = mySlots.allowPlaceOrder

      if (mySlots.timeSlots.length > 0) {
        this.takeawayTimeSlot = mySlots.timeSlots[0]
        this.shopclosed = false
      } else {
        this.shopClosed = true
        this.takeawayTimeSlot = this.ml.shopclosed
      }
    },
    onOrderConfirmedFromServerToWeb(order, addremove) {
      if (addremove !== '1') {
        return
      }
      if (this.$refs.mySendButton !== undefined) {
        this.$refs.mySendButton.stop()
      }
      this.showWaiting = false
      // this.hidecheckout() /* sometime click on menu tab, show checkout, hide checkout here */
      this.$Modal.success({
        title: this.ml.success,
        content: this.ml.ordersendsuccess,
        okText: this.ml.ok,
        onOk: () => {
          this.hidecheckout()
          setTimeout(() => {
            this.$router.push('admin')
          }, 300)
          setTimeout(() => {
            console.log('onOrderConfirmedFromServerToWeb')
            this.$root.eventHub.$emit('checkout.onOrderConfirmedFromServerToWeb', order, addremove)
          }, 1000)
        }
      });
    },
    sendOrder() {
      if (this.selectFoods.length === 0) {
        return
      }
      if (this.takeawayTimeSlot === this.ml.selecttime) {
        this.$Modal.success({
          content: this.ml.selecttime,
          okText: this.ml.ok
        });
        return
      }
      // this.showWaiting = true
      // this.busyWithSending = true
      // this.startTime = new Date()
      // this.mySendingTimer = setInterval(this.checkSending, 1000)
      if (this.shopClosed) {
        this.$Modal.success({
          content: this.ml.restaurantisalreadyclosed,
          okText: this.ml.ok
        });
      } else {
        this.$refs.mySendButton.start()
        this.$root.eventHub.$emit('signalr.sendOrder', this.orderRequestString)
      }
    },
    timeSlotSelected(timeSlot) {
      console.log(timeSlot)
      this.takeawayTimeSlot = timeSlot
    },
    onSessionExpired(obj) {
      console.log('onSessionExpired in checkout')
      if (this.$refs.mySendButton !== undefined) {
        this.$refs.mySendButton.stop()
      }
      this.showWaiting = false
      this.$Modal.warning({
        content: this.ml.restaurantisalreadyclosed,
        okText: this.ml.ok,
        onOk: () => {
          this.hidecheckout()
        }
      });
    }
  }
}

</script>

<style lang="stylus" scoped>

* {
  box-sizing: border-box;
}
.billcontainer {
  display: flex;
  flex-wrap: nowrap;
}
.billcontent {
  flex: 80%
}

.rowheader {
  display: flex;
  flex-wrap: nowrap;
  margin-bottom : 20px;
  padding-bottom : 10px;
  padding-top: 50px;
  border-bottom : 1px dashed
}
.footline {
  display: flex;
  flex-wrap: nowrap;
  padding-top: 20px;
  border-top : 1px dashed
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: nowrap;
  margin-bottom : 10px;
}

/* Create four equal columns */
.columncount {
  flex: 10%;
  //text-align right;
  //padding-right 20px;
}

.columnname {
  flex: 70%;
}
.columnunitprice {
  flex: 10%;
}
.columnprice {
  flex: 10%;
}
.columnpadding {
  flex : 10%
}
.totalline
{
  margin-top: 40px
  text-align: center;
  font-size: 24px;
}
.openinghourcomment
{
  margin-top: 10px
  text-align: center;
}
.ordercomment
{
  margin-top: 10px
}
.ordercommenttext {
  float: left;
  width: 100%;
  padding: 2px 5px;
  outline: none;
  resize: none;
}
.ordertext {
  margin-top: 10px;
  text-align: center;
}
.buttonarea
{
  text-align: center;
  margin-top: 70px;
}
.close {
  text-align: center;
  padding: 5px 10px;
  font-size: 12px;
  font-weight: 700;
  background: #00b43c;
  color: white;
  line-height: 48px;
}
.backdrop {
  z-index 150
}
.spin {
  position: absolute;
  top: 50%;
  left: 50%;
  margin-right: -50%;
  transform: translate(-50%, -50%);
  z-index: 160;
  width: 100px;
  height: 100px;
  background solid red;
}
.timeSlotWrapper
  border solid 1px
  height 40px
  border-radius 2px

.timeSlotContainer
  margin 12px 5px
  line-height 20px
  vertical-align middle
  text-align center
  // background red
  font-size 16px
.detail
  position fixed
  left 0
  top 0
  height 100%
  width 100%
  z-index 130
  background rgba(7,17,27,0.8)
  // backdrop-filter blur(2px)
  transition all 0.4s ease
@media screen and (min-width: 800px)
  .detail
      position fixed
      left 0
      top 0
      height 100%
      width 100%
      z-index 130
      background rgba(7,17,27,0.8)
      // backdrop-filter blur(2px)
      transition all 0.4s ease
.detailWrapper
  position fixed
  left 0
  top 0
  bottom 48px
  width 100%
  z-index 130
  background white
  color rgb(77,85,93)
  transition all 0.4s ease
  &.move-enter-avtive,&.move-leave{
    transform translate3d(0,0,0)
  }
  &.move-enter,&.move-leave-active{
    transform translate3d(100%,0,0)
  }
@media screen and (min-width: 800px)
  .detailWrapper
    position fixed
    top 0
    left 20%
    margin-right -20%
    bottom 48px
    width 100%
    max-width: 800px
    background white
    color rgb(77,85,93)
    transition all 0.4s ease
    &.move-enter-avtive,&.move-leave{
      transform translate3d(0,0,0)
    }
    &.move-enter,&.move-leave-active{
      transform translate3d(10%,0,0)
    }
 .detail-close
    position relative
    width 32px
    height 32px
    margin -64px auto 0 auto
    clear both
    font-size 32px
    color rgba(0,0,0,0.5)


</style>
