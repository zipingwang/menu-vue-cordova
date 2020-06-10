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
        .foodlinename
          margin 0px
  .map
    text-align center
    margin-top 20px
    margin-bottom 100px

</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper">
    <login class="login" v-if="!loggedIn">
    </login>
    <div class="ratings-content" v-if="loggedIn">
      <div class="divider"></div>
      <div class="row">
        <div class="column" v-for="order in orders">
           <div class="ricetableblock">
                <div class="title">
                    <p>{{order.orderid}} {{order.datetime}} {{order.totalprice}} </p>
                    {{order.customerinfo}}
                    {{order.info}}
                </div>
                <table class="foodlineblock">
                  <tr class="foodline" v-for="(orderline, index) in order.orderlines">
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
      loggedIn: false,
      selectedOrder: {},
      ricetables: this.data.ricetables,
      orders: [{
        'orderid': '123',
        'datetime': '18:50',
        'totalprice': '20',
        'customerinfo': 'customer',
        'info': 'this is takeaway',
        orderlines: [{
          'nr': '100',
          'count': '2',
          'name': 'peking eend',
          'price': '12'
        }]},
      {
        'orderid': '345',
        'datetime': '16:20',
        'totalprice': '50',
        'customerinfo': 'customer',
        'info': 'this is takeaway',
        orderlines: [{
          'nr': '100',
          'count': '2',
          'name': 'peking eend',
          'price': '12'
        },
        {
          'nr': '200',
          'count': '1',
          'name': 'Kip groeten',
          'price': '30'
        }]}
      ]
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
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
      alert('close order' + JSON.stringify(order))
    },
    ok() {
      this.$Message.info(JSON.stringify(this.selectedOrder));
      this.selectedOrder = {}
    },
    cancel() {
      this.$Message.info('Clicked cancel');
      this.selectedOrder = {}
    }
  }
}

</script>
