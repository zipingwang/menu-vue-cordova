<template lang="html">
  <div>
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
                <span class="columncount">{{trans.ordercount}}</span>
                <span class="columnname">{{trans.orderdescription}}</span>
                <span class="columnunitprice">{{trans.orderunitprice}}</span>
                <span class="columnprice">{{trans.orderlinetotal}}</span>
              </li>
              <li class="row" v-for="food in selectFoods">
                <span class="columncount">{{food.count}}</span>
                <span class="columnname">{{food.name}}</span>
                <span class="columnunitprice">{{food.price}} </span>
                <span class="columnprice">{{food.price * food.count}} </span>
              </li>
              <li class="footline">
                <span class="columncount">{{totalCount}}</span>
                <span class="columnname"></span>
                <span class="columnunitprice"></span>
                <span class="columnprice">{{totalPrice}}</span>
              </li>
            </ul>
            <div class="totalline">{{trans.ordertotal}}        €{{totalPrice}}</div>
            <div class="ordertext" v-if="!seller.supportOnlineOrder">
              U kan deze bestelling plaatsen door te bellen naar 051207637
            </div>
            <div class="buttonarea">
              <span class="close" @click="sendOrder()" v-if="seller.supportOnlineOrder">OK</span>
              <!-- <button type="button" @click="connect()" v-if="seller.supportOnlineOrder">Ok</button> -->
              <span class="close" @click="hidecheckout()">Sluiten</span>
              <!-- <button type="button" @click="hidecheckout()">Cancel</button> -->
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

export default {
  components: {
  },
  props: {
    seller: {},
    selectFoods: {
      type: Array,
      default: []
    },
    totalPrice: 0,
    ml: {}
  },
  data() {
    return {
      show: false,
      // trans: ml.trans, /* ml without this. it search from global js. in this case from data.js */
      trans: this.ml,
      url: this.seller.sellerurl,
      userName: 'vue app',
      simpleHubProxy: null,
      connectionId: ''
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
      requestString = requestString + '@@@Option1@@Today@@18:25@@@'
      requestString = requestString + orderline + '@@@'
      return requestString
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    // this.connectToSignalRServer()
    this.$root.eventHub.$on('signalr.onOrderConfirmedFromServerToWeb', this.onOrderConfirmedFromServerToWeb)
    console.log('checkout JSON.stringify(this.trans)')
    console.log(JSON.stringify(this.trans))
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
    onOrderConfirmedFromServerToWeb(orderId) {
      this.$Modal.success({
        title: 'Success',
        content: '<p>Order is geplaast</p> in checkout'
      });
    },
    sendOrder() {
      this.$root.eventHub.$emit('signalr.sendOrder', this.orderRequestString);
      // this.simpleHubProxy.server.orderFromWebToServer(this.connectionId, 'vue', this.orderRequestString);
    },
    connect2() {
      alert('click');
      let url = 'http://localhost/SendOrder.ashx'
      // axios.post(url, {
      //   order: '1@@2@@vital@@wang', /* server side can't get order parameter */
      //   lastName: 'Williams'
      // })
      // .then((response) => {
      //   alert('response from server 1');
      //   console.log(response);
      // }, (error) => {
      //   console.log(error);
      // })
      $.post(url,
        {
          order: this.requestString,
          city: 'Duckburg'
        },
        function (data, status) {
          alert('Data: ' + data + '\nStatus: ' + status);
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
.ordertext {
  margin-top: 10px;
  text-align: center;
}
.buttonarea
{
  text-align: center;
  margin-top: 20px;
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
