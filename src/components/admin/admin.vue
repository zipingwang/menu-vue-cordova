<style lang="stylus" scoped>
* {
  box-sizing: border-box;
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: wrap;
}

/* Create four equal columns */
/* maximum 4 column, if screen width is not enough, it will display less column, e.g. 3 or 2 or 1, new row will made. */
.column {
  flex: 25%;
  padding: 20px;
}

/* On screens that are 992px wide or less, go from four columns to two columns */
@media screen and (max-width: 992px) {
  .column {
    flex: 50%;
  }
}

/* On screens that are 600px wide or less, make the columns stack on top of each other instead of next to each other */
@media screen and (max-width: 600px) {
  .row {
    flex-direction: column;
  }
}
.button
  text-align: center;
  padding: 5px 10px;
  font-size: 12px;
  font-weight: 700;
  background: #00b43c;
  color: white;
  line-height: 48px;
.ratingsWrapper
  position: absolute
  top: 224px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden
  .buttonarea
    margin 8px 10px
.ratings-content
  .row
    .column
      text-align center
      color rgb(77,85,93)
      .ricetableblock
        border 1px solid #f3f5f7
        border-radius: 10px;
        height: 100%
        box-sizing border-box
        .orderheader
          font-size 24px
          .orderNotViewed
            background-color green
          .orderViewed
            background-color  red
        .title
          font-size 24px
          line-height 24px
          margin-top 20px
        .pricelineblock
          .priceline
            font-size 16px
            line-height 16px
            margin-top 20px
        .foodblock
          margin-top 50px
          .foodlineblock
            .foodline
              font-size 16px
              height 16px
            .seperator
              margin-top 5px
              height 30px
              font-size 16px
              line-height 30px
        .endlineblock
          .endline
            margin-top 50px
            font-size 16px
            height 30px
            line-height 30px
            margin-bottom 20px
            padding-bottom 20px
        .endfilling
            height 20px
        .foodlineblock
          margin 10px 50px
          width 80%
          padding 0px 50px 0px 0px
          border-radius 5px
        .foodline
          padding 5px 10px
          background #f2f2f2
          border-radius 10px
        .foodlinecount
          width 20px
          text-align right
        .foodlinename
          margin 0px
  .map
    text-align center
    margin-top 20px
    margin-bottom 100px

</style>

<template lang="html">
  <div class="ratings-content" v-if="loggedIn">
      <div class="buttonarea"  v-if="isAdmin" >
          <i-button size="small" type="primary"  @click= "downloadOrder()" icon="ios-download-outline">{{ml.checkorder}}</i-button>
          <i-button size="small" :type="restaurantButtonType" @click= "getRestaurant()">{{ml.restaurant}} {{restaurantCount}}</i-button>
          <i-button size="small" :type="restaurantButtonType" @click= "getRestaurant()" type="warning" shape="circle" v-if="restauranNotViewedtCount > 0">{{restauranNotViewedtCount}}</i-button>
          <i-button size="small" :type="takeawayButtonType"  @click= "getTakeaway()">{{ml.takeaway}} {{takeawayCount}}</i-button>
          <i-button size="small" :type="takeawayButtonType" @click= "getTakeaway()" type="warning" shape="circle" v-if="takeawayNotViewedCount > 0">{{takeawayNotViewedCount}}</i-button>
          <i-button size="small" :type="allButtonType" @click= "getAll()">{{ml.all}} {{allCount}}</i-button>
      </div>
  <div class="divider"></div>
  <drawer ref="menuDrawer" placement="right" :closable="true" v-if="data.options.isAdmin === '1'" v-model="contextMenuVisible">
      <p class="drawbutton"><i-button @click="closeOrder" type="primary">{{ml.close}}</i-button></p>
      <p class="drawbutton"><i-button @click="deleteOrder" type="primary">{{ml.delete}}</i-button></p>
  </drawer>
  <div class="ratingsWrapper" ref="ratingsWrapper">
<!--
    <login ref= "myLogin" :seller="seller" :data="data" :ml="ml">
    </login> -->

      <div class="row">
        <div class="column" v-for="order in visibleOrders">
           <div class="ricetableblock">
              <row :gutter="16" class="orderheader">
                <i-col span="3">
                    <div>{{order.tableNr}}</div>
                </i-col>
                <i-col span="3">
                    <Icon type="md-briefcase" v-if = "order.isTakeaway === '1'" />
                    <Icon type="ios-restaurant" v-if = "order.isTakeaway === '0'" />
                </i-col>
                <i-col span="5">
                    <div>{{getOrderTimeString(order)}}</div>
                </i-col>
                <i-col span="4">
                    <div>{{order.totalPrice}}€</div>
                </i-col>
                <i-col span="6" v-if="isAdmin">
                    <i-button size="small" @click= "changeOrder(order)" :type="order.orderViewed === false ? 'warning' : 'default'" icon="ios-menu" ></i-button>
                    <!-- <dropdown style="margin-left: 10px" @on-click="changeOrder($event, order)">
                    <i-button type="primary" icon="ios-menu"></i-button>
                    <dropdown-menu slot="list">
                        <dropdown-item name="closeOrder">{{ml.close}}</dropdown-item>
                        <dropdown-item divided name="deleteOrder">{{ml.delete}}</dropdown-item>
                    </dropdown-menu> -->
                </dropdown>
                     <!-- <i-button type="primary" size="small" shape="circle" icon="ios-menu" @click="closeOrder(order)"></i-button> -->
                </i-col>
            </row>
                <!-- <div class="title">
                    <p>{{order.tableNr}} {{getTakeawayString(order)}} {{order.orderTime}} {{order.totalPrice}} </p>
                    {{order.customerInfo}}
                    {{order.info}}
                </div> -->
                <table class="foodlineblock">
                  <tr class="foodline" v-for="(orderline, index) in order.orderLines">
                    <td class="foodlinecount">{{orderline.count}}</td>
                    <td class="foodlinename">{{getMenuName(orderline.nr)}}</td>
                  </tr>
                </table>
                <!-- <span class="button" @click="closeOrder(order)">close</span> -->
                <modal
                  ref="dialog"
                  v-model="modalCloseOrder"
                  :closable="false"
                  :ok-text="ml.ok"
                  :cancel-text="ml.cancel"
                  @on-ok="ConfirmCloseOrder"
                  @on-cancel="CancelCloseOrder">
                   <p>
                    <icon type="md-information-circle"></icon>
                    <span>{{ml.askconfirmcloseorder}}</span>
                  </p>
                </modal>
                <modal
                  ref="dialog"
                  v-model="modalDeleteOrder"
                  :closable="false"
                  :ok-text="ml.ok"
                  :cancel-text="ml.cancel"
                  @on-ok="ConfirmDeleteOrder"
                  @on-cancel="CancelDeleteOrder">
                   <p>
                    <icon type="md-information-circle"></icon>
                    <span>{{ml.askconfirmdeleteorder}}</span>
                  </p>
                </modal>
                <!-- <div class="foodblock">
                    <ul>
                        <li class="foodlineblock" v-for="(orderline, index) in order.orderlines">
                            <div class="foodline">{{orderline.count}}{{orderline.name}}</div>
                            <div class="seperator" v-if="index != order.orderlines.length - 1">***</div>
                        </li>
                    </ul>
                </div> -->
                <div class="endfilling"></div>
            </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import star from 'components/star/star'
import login from 'components/login/login'
import BScroll from 'better-scroll'

export default {
  components: {
    star,
    login
  },
  props: {
    data: {},
    ml: {},
    seller: {}
    // orders: {}
  },
  data() {
    return {
      modalCloseOrder: false,
      modalDeleteOrder: false,
      selectedOrder: {},
      ricetables: this.data.ricetables,
      orders: [],
      restaurantButtonType: 'default',
      takeawayButtonType: 'default',
      allButtonType: 'primary',
      currentOrderType: 'all',
      firstDownLoaded: true,
      contextMenuVisible: false
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    // this._initScroll(); // 初始化scroll
    this.$root.eventHub.$on('signalr.onOrderDownloaded', this.onOrderDownloaded)
    this.$root.eventHub.$on('signalr.onBroadcastOrder', this.onBroadcastOrder)
    this.$root.eventHub.$on('checkout.onOrderConfirmedFromServerToWeb', this.onOrderConfirmedFromServerToWeb)
    console.log('admin vue created')


    // this.downloadOrder()
    // this.$root.eventHub.$on('login.loggedin', this.onloggedin)
  },
  computed: {
    loggedIn() {
      // return this.data.options.isAdmin === '1'
      return this.data.options.cusId !== '' && this.data.options.cusId !== '-1' && this.data.options.cusId !== '-2'
    },
    isAdmin() {
      return this.data.options.isAdmin === '1' && this.data.options.cusId !== ''
    },
    visibleOrders() {
      return this.orders.filter(
        (order) => {
          if (this.currentOrderType === 'restaurant') {
            return order.isTakeaway === '0'
          } else if (this.currentOrderType === 'takeaway') {
            return order.isTakeaway === '1'
          } else {
            return true
          }
        }
      )
    },
    takeawayCount() {
      return this.orders.filter(
        (order) => {
          return order.isTakeaway === '1'
        }
      ).length
    },
    takeawayNotViewedCount() {
      return this.orders.filter(
        (order) => {
          return order.isTakeaway === '1' && order.orderViewed === false
        }
      ).length
    },
    restaurantCount() {
      return this.orders.filter(
        (order) => {
          return order.isTakeaway === '0'
        }
      ).length
    },
    restauranNotViewedtCount() {
      return this.orders.filter(
        (order) => {
          return order.isTakeaway === '0' && order.orderViewed === false
        }
      ).length
    },
    allCount() {
      return this.Orders ? 0 : this.orders.length
    }
  },
  watch: {
    '$route' (to, from) {
      // react to route changes...
      // alert('route')
      // console.log('route')
      // console.log(to.path)
      // console.log(from)
      // console.log(this.data.options)
      if (to.path === '/admin') {
        if (this.data.options.isAdmin === '1' && this.firstDownLoaded) {
          this.downloadOrder()
          this.firstDownLoaded = false
        } else {
          // this.$refs.myLogin.showlogin()
        }
      }
      // this.$nextTick(() => {
      //   this._initScroll(); // 初始化scroll
      // })
    },
    contextMenuVisible(newValue, oldValue) {
      if (newValue === false) {
        this.selectedOrder = {}
      }
    }
  },
  methods: {
    _initScroll() {
      // alert('ok')
      // console.log(`_initScrollseller ${this.data.monthmenu.title},`)
      let smallScreen = screen.width <= 800;
      console.log(`screen width ${screen.width}, smallScreen ${smallScreen}`)
      this.foodsScroll = new BScroll(this.$refs.ratingsWrapper, {
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
    closeOrder() {
      this.modalCloseOrder = true
      // this.$root.eventHub.$emit('signalr.closeOrder', order.orderId)
      // alert('close order' + JSON.stringify(order))
    },
    ConfirmCloseOrder() {
      // this.$Message.info(JSON.stringify(this.selectedOrder));
      this.$root.eventHub.$emit('signalr.closeOrder', this.selectedOrder.orderId)
      // this.selectedOrder = {}
      this.modalCloseOrder = false
    },
    CancelCloseOrder() {
      // this.$Message.info('Clicked cancel');
      // this.selectedOrder = {}
      this.modalCloseOrder = false
    },
    downloadOrder() {
      // alert('this.$root.eventHub.$emit(signalr.downloadOrder)')
      if (this.data.options.cusId !== '') {
        this.$root.eventHub.$emit('signalr.downloadOrder')
      }
    },
    onOrderDownloaded(orders) {
      // alert('orderDownloaded in admin')
      // console.log(typeof orders);
      this.orders = orders.orders
      this.$nextTick(() => {
        this._initScroll(); // 初始化scroll
      })
    },
    onloggedin(cus) {
      // console.log(cus)
      // this.downloadOrder()
    },
    onBroadcastOrder(messageBody) {
      console.log('onBroadcastOrder in admin.vue')
      this.doAddRemoveOrder(messageBody.order, messageBody.addOrRemove)
    },
    onOrderConfirmedFromServerToWeb(orderString, addremove) {
      console.log('onOrderConfirmedFromServerToWeb')
      this.doAddRemoveOrder(orderString, addremove)
    },
    doAddRemoveOrder(orderString, addremove) {
      console.log('doAddRemoveOrder')
      // debugger
      if (addremove === '1') {
        let flag = false
        let order = JSON.parse(orderString)
        console.log(order)
        order.orderViewed = false
        for (let index = 0; index < this.orders.length; index++) {
          if (this.orders[index].orderId === order.orderId) {
            flag = true
            this.$set(this.orders, index, order) /* replace existing order */
            // this.orders[index] = order /* not update by vue */
            break
          }
        }
        if (!flag) {
          this.orders.push(order)
          // downloadOrder()
        }
      } else if (addremove === '-1' || addremove === '-2') { /* -1 close, -2 delete order */
        let flag = false
        let orderId = orderString
        for (let index = 0; index < this.orders.length; index++) {
          if (this.orders[index].orderId === orderId) {
            this.orders.splice(index, 1);
            // alert('found')
            break
          }
        }
      }
      this.$nextTick(() => {
        this.foodsScroll.refresh()
        // this._initScroll(); // 初始化scroll
      })
      this.playSound()
    },
    getTakeawayString(order) {
      if (order.isTakeaway === '0') {
        return this.ml.in
      } else {
        return this.ml.out
      }
    },
    getOrderTimeString(order) {
      if (order.isTakeaway === '0') {
        console.log(order.orderTime)
        console.log(order.orderTime.length)
        if (order.orderTime.length === 14) {
          console.log(order.orderTime.substring(10, 12))
          console.log(order.orderTime.substring(12, 14))
          return order.orderTime.substring(10, 12) + ':' + order.orderTime.substring(12, 14)
        } else {
          return order.orderTime
        }
      } else {
        return order.takeawayTime
      }
    },
    getRestaurant() {
      this.restaurantButtonType = 'primary'
      this.takeawayButtonType = 'default'
      this.allButtonType = 'default'
      this.currentOrderType = 'restaurant'
    },
    getTakeaway() {
      this.restaurantButtonType = 'default'
      this.takeawayButtonType = 'primary'
      this.allButtonType = 'default'
      this.currentOrderType = 'takeaway'
    },
    getAll() {
      this.restaurantButtonType = 'default'
      this.takeawayButtonType = 'default'
      this.allButtonType = 'primary'
      this.currentOrderType = 'all'
    },
    deleteOrder() {
      this.modalDeleteOrder = true
    },
    ConfirmDeleteOrder() {
      // this.$Message.info(JSON.stringify(this.selectedOrder));
      this.$root.eventHub.$emit('signalr.deleteOrder', this.selectedOrder.orderId)
      this.modalDeleteOrder = false
      this.contextMenuVisible = false
    },
    CancelDeleteOrder() {
      this.modalDeleteOrder = false
    },
    changeOrder(order) {
      console.log('change order')
      order.orderViewed = true
      this.contextMenuVisible = true
      this.selectedOrder = order
    },
    playSound() {
      if (this.isAdmin) {
        console.log('playsound')
        var audio = new Audio('static/sound/sound.mp3');
        audio.play();
      }
    },
    getMenuName(menuNr) {
      console.log(menuNr)
      let menuInDataJs = {}
      this.data.goods.forEach(menuGroup => {
        menuGroup.foods.forEach(menuItem => {
          if (menuItem.menunr === menuNr) {
            menuInDataJs = menuItem
          }
        });
      });
      console.log(menuInDataJs)
      return menuInDataJs.name[data.currentlnindex]
    }
  }
}

</script>
