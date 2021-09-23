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
  // flex: 25%;
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
        max-width 500px
        min-width 350px
        .orderheader
          font-size 24px
          // background-color red
          .orderNotViewed
            background-color green
          .orderViewed
            background-color  red
        .orderheaderline
          font-size 14px
          text-align left
          padding-left 10px
          // background-color red
          .orderNotViewed
            background-color green
          .orderViewed
            background-color  red
        .orderheaderblock
          margin 10px
          padding 0px 50px 0px 0px
          border-radius 5px
          .orderheaderline
            padding 5px 10px
            background #f2f2f2
            border-radius 10px
            .orderheaderlinename
              font-size 14px
              text-align left
              padding-left 20px
            .orderheaderlinevalue
              font-size 14px
              text-align left
              padding-left 5px
        .foodlineblock
          margin 10px
          width 90%
          padding 0px 50px 0px 0px
          border-radius 5px
          .foodline
            padding 5px 10px
            background #f2f2f2
            border-radius 10px
            .foodlinecount
              width 30px
              text-align left
              padding-left 10PX
            .foodlinename
              margin 0px
              text-align left
            .foodlineprice
              margin 0px
              text-align right
        &.selectedOrder
          border-color #00b43c;
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

      <div class="row">
        <div class="column" v-for="order in visibleOrders">
           <div class="ricetableblock" :class="{'selectedOrder':order==selectedOrder}">
              <row :gutter="16" class="orderheader">
                <i-col span="13">
                    <div>{{ml.ordersequencenumber}} {{order.tableNr}}</div>
                </i-col>
                <i-col span="2" v-if="isAdmin">
                    <Icon type="md-briefcase" v-if = "order.isTakeaway === '1'" />
                    <Icon type="ios-restaurant" v-if = "order.isTakeaway === '0'" />
                </i-col>
                <!-- <i-col span="8">
                    <div>{{getOrderTimeString(order)}}</div>
                </i-col>
                <i-col span="5">
                    <div>€{{order.totalPrice}}</div>
                </i-col> -->
                <i-col span="3" v-if="isAdmin">
                    <i-button size="small" @click= "changeOrder(order)" :type="order.orderViewed === false ? 'warning' : 'default'" icon="ios-menu" ></i-button>
                </dropdown>
                </i-col>
              </row>
              <!-- <row :gutter="16" class="orderheaderline">
                <i-col span="16">
                    <div>{{ml.ordersequencenumber}}: {{order.tableNr}}</div>
                </i-col>
              </row>
              <row :gutter="16" class="orderheaderline">
                <i-col span="16">
                    <div>{{ml.takeawaytime}}: {{getOrderTimeString(order)}}</div>
                </i-col>
              </row>
              <row :gutter="16" class="orderheaderline">
                <i-col span="16">
                    <div>{{ml.ordertotalamount}}: €{{order.totalPrice}}</div>
                </i-col>
              </row> -->
               <table class="orderheaderblock">
                  <!-- <tr class="orderheaderline">
                    <td class="orderheaderlinename">{{ml.ordersequencenumber}}:</td>
                    <td class="orderheaderlinevalue"> {{order.tableNr}}</td>
                  </tr> -->
                  <tr class="orderheaderline">
                    <td class="orderheaderlinename">{{ml.takeawaytime}}:</td>
                    <td class="orderheaderlinevalue">{{getOrderTimeString(order)}}</td>
                  </tr>
                  <tr class="orderheaderline">
                    <td class="orderheaderlinename">{{ml.ordertotalamount}}:</td>
                    <td class="orderheaderlinevalue"> € {{(order.totalPrice * 1).toFixed(2)}}</td>
                  </tr>
                </table>
                <table class="foodlineblock">
                  <tr class="foodline" v-for="(orderline, index) in order.orderLines">
                    <td class="foodlinecount">{{orderline.count}}</td>
                    <td class="foodlinename">{{orderline.name}}</td>
                    <td class="foodlineprice">{{(orderline.price * orderline.count).toFixed(2)}}</td>
                  </tr>
                </table>
              <div>{{order.orderComment}}</div>
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
      orderDownloaded: false,
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
    this.$root.eventHub.$on('login.loggedOut', this.onLoggedOut)
    console.log('admin vue created')
    this.downloadOrder()
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
      if (to.path === '/admin') {
        if (this.data.options.cusId !== '' && !this.orderDownloaded) {
          this.downloadOrder()
        }
      }
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
      this.$root.eventHub.$emit('signalr.closeOrder', this.selectedOrder.customerOrderId)
      // this.selectedOrder = {}
      this.modalCloseOrder = false
    },
    CancelCloseOrder() {
      // this.$Message.info('Clicked cancel');
      // this.selectedOrder = {}
      this.modalCloseOrder = false
    },
    downloadOrder() {
      console.log('downloadOrder in admin.vue')
      // if (this.data.options.cusId !== '' && !this.orderDownloaded) {
      //   this.$root.eventHub.$emit('signalr.downloadOrder')
      // }
      this.$root.eventHub.$emit('signalr.downloadOrder')
    },
    onOrderDownloaded(orders) {
      // alert('orderDownloaded in admin')
      console.log('onOrderDownloaded in admin');
      // console.log(orders)
      this.orders = orders.orders
      this.orderDownloaded = true
      this.getAll() /* reset filter */
      this.$nextTick(() => {
        this._initScroll(); // 初始化scroll
      })
    },
    onloggedin(cus) {
      // console.log(cus)
      // this.downloadOrder()
    },
    onLoggedOut(cus) {
      this.orderDownloaded = false
      this.selectedOrder = {}
      this.orders.splice(0, this.orders.length)
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
      console.log(orderString)
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
        console.log('orderString')
        console.log(orderString)
        console.log(this.orders)
        for (let index = 0; index < this.orders.length; index++) {
          if (this.orders[index].customerOrderId === orderId) {
            this.orders.splice(index, 1);
            break
          }
        }
      }
      this.$nextTick(() => {
        this.foodsScroll.refresh()
        // this._initScroll(); // 初始化scroll
      })
      this.contextMenuVisible = false
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
        if (order.orderTime.length === 14 || order.orderTime.length === 12) {
          console.log(order.orderTime.substring(8, 10))
          console.log(order.orderTime.substring(10, 12))
          return order.orderTime.substring(8, 10) + ':' + order.orderTime.substring(10, 12)
        } else {
          return order.orderTime
        }
      } else {
        console.log(order.takeawayTime)
        if (order.takeawayTime && (order.takeawayTime.length === 14 || order.takeawayTime.length === 12)) {
          console.log(order.takeawayTime.substring(8, 10))
          console.log(order.takeawayTime.substring(10, 12))
          return order.takeawayTime.substring(8, 10) + ':' + order.takeawayTime.substring(10, 12)
        } else {
          return order.takeawayTime
        }
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
      this.$root.eventHub.$emit('signalr.deleteOrder', this.selectedOrder.customerOrderId)
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
    }
    // getMenuName(menuNr) {
    //   console.log(menuNr)
    //   let menuInDataJs = {}
    //   this.data.goods.forEach(menuGroup => {
    //     menuGroup.foods.forEach(menuItem => {
    //       if (menuItem.menunr === menuNr) {
    //         menuInDataJs = menuItem
    //       }
    //     });
    //   });
    //   console.log(menuInDataJs)
    //   return menuInDataJs.name[data.currentlnindex]
    // }
  }
}

</script>
