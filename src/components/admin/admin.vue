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
  top: 174px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden
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
  <div class="ratingsWrapper" ref="ratingsWrapper">
<!--
    <login ref= "myLogin" :seller="seller" :data="data" :ml="ml">
    </login> -->
    <div class="ratings-content" v-if="loggedIn">
      <div><span class="button" @click= "downloadOrder">check order</span></div>
      <div class="divider"></div>
      <div class="row">
        <div class="column" v-for="order in orders">
           <div class="ricetableblock">
                <div class="title">
                    <p>{{order.tableNr}} {{getTakeawayString(order)}} {{order.orderTime}} {{order.totalPrice}} </p>
                    {{order.customerInfo}}
                    {{order.info}}
                </div>
                <table class="foodlineblock">
                  <tr class="foodline" v-for="(orderline, index) in order.orderLines">
                    <td class="foodlinecount">{{orderline.count}}</td>
                    <td class="foodlinename">{{orderline.name}}</td>
                  </tr>
                </table>
                <span class="button" @click="closeOrder(order)">close</span>
                <modal
                  ref="dialog"
                  v-model="modal1"
                  title="Common Modal dialog box title"
                  @on-ok="ok"
                  @on-cancel="cancel">
                  <p>Content of dialog</p>
                  <p>Content of dialog</p>
                  <p>Content of dialog</p>
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
      modal1: false,
      selectedOrder: {},
      ricetables: this.data.ricetables,
      orders: []
      // orders: [{
      //   'orderid': '123',
      //   'datetime': '18:50',
      //   'totalprice': '20',
      //   'customerinfo': 'customer',
      //   'info': 'this is takeaway',
      //   orderlines: [{
      //     'nr': '100',
      //     'count': '2',
      //     'name': 'peking eend',
      //     'price': '12'
      //   }]},
      // {
      //   'orderid': '345',
      //   'datetime': '16:20',
      //   'totalprice': '50',
      //   'customerinfo': 'customer',
      //   'info': 'this is takeaway',
      //   orderlines: [{
      //     'nr': '100',
      //     'count': '2',
      //     'name': 'peking eend',
      //     'price': '12'
      //   },
      //   {
      //     'nr': '200',
      //     'count': '1',
      //     'name': 'Kip groeten',
      //     'price': '30'
      //   }]}
      // ]
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    this.$root.eventHub.$on('signalr.orderDownloaded', this.orderDownloaded)
    this.$root.eventHub.$on('signalr.onOrderConfirmedFromServerToWeb', this.onOrderConfirmedFromServerToWeb)
    // this.$root.eventHub.$on('login.loggedin', this.onloggedin)
  },
  computed: {
    loggedIn() {
      return this.data.options.isAdmin === '1'
      // return this.data.options.cusId !== '' && this.data.options.cusId !== '-1' && this.data.options.cusId !== '-2'
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
      // if (to.path === '/admin') {
      //   if (this.data.options.isAdmin === '1') {
      //     this.downloadOrder()
      //     this.$refs.myLogin.hidelogin()
      //   } else {
      //     this.$refs.myLogin.showlogin()
      //   }
      // }
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
    closeOrder(order) {
      this.modal1 = true
      this.selectedOrder = order
      this.$root.eventHub.$emit('signalr.closeOrder', order.orderId)
      alert('close order' + JSON.stringify(order))
    },
    ok() {
      this.$Message.info(JSON.stringify(this.selectedOrder));
      this.selectedOrder = {}
    },
    cancel() {
      this.$Message.info('Clicked cancel');
      this.selectedOrder = {}
    },
    downloadOrder() {
      alert('this.$root.eventHub.$emit(signalr.downloadOrder)')
      this.$root.eventHub.$emit('signalr.downloadOrder')
    },
    orderDownloaded(orders) {
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
    onOrderConfirmedFromServerToWeb(orderString, addremove) {
      alert('onOrderConfirmedFromServerToWeb in admin')
      var order = JSON.parse(orderString)
      console.log(order)
      debugger
      if (addremove === '1') {
        let flag = false
        for (let index = 0; index < this.orders.length; index++) {
          if (this.orders[index].orderId === order.orderId) {
            flag = true
            break
          }
        }
        if (!flag) {
          this.orders.push(order)
          alert('add')
        }
      } else if (addremove === '-1') {
        let flag = false
        for (let index = 0; index < this.orders.length; index++) {
          if (this.orders[index].orderId === order.orderId) {
            this.orders.splice(index, 1);
            alert('found')
            break
          }
        }
      }
    },
    getTakeawayString(order) {
      if (order.isTakeaway === '0') {
        return ''
      } else {
        return '外卖'
      }
    }
  }
}

</script>
