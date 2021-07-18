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

.ratingsWrapper
  position: absolute
  top: 174px
  bottom: 48px
  left: 0
  width: 100%
  overflow: hidden
.ratings-content
  .row
    .column
      text-align center
      color rgb(77,85,93)
      .ricetableblock
        position relative
        border 1px solid #f3f5f7
        border-radius: 10px;
        height: 100%
        box-sizing border-box
        .title
          font-size 24px
          line-height 24px
          margin-top 30px
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
            height 50px
        .shopCart
          position absolute
          right 18px
          top 8px
          height 24px
          text-align center
          z-index 2
          .text
            box-sizing border-box
            height 100%
            line-height 24px
            color white
            font-size 10px
            padding 0 12px
            border-radius 12px
            background rgb(0,160,220)
            &.fade-enter-active,&.fade-leave-active{
              transition opacity .2s
            }
            &.fade-enter,&.fade-leave-active{
              opacity 0
            }
        .cartcontrol
          position absolute
          right 12px
          top 2px
  .map
    text-align center
    margin-top 20px
    margin-bottom 100px

</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper">
    <div class="ratings-content">
      <!-- <div class="divider"></div> -->
      <div class="row">
        <div class="column">
           <div class="ricetableblock">
                <div class="title">
                  {{title}}
                </div>
                <div class="pricelineblock">
                    <ul>
                        <li class="priceline" v-for="priceline in pricelines">
                            <div class="priceline">{{priceline}}</div>
                        </li>
                    </ul>
                </div>
                <div class="foodblock">
                    <ul>
                        <li class="foodlineblock" v-for="(food, index) in foodlines">
                            <div class="foodline">{{food}}</div>
                            <div class="seperator" v-if="index != foodlines.length - 1">***</div>
                        </li>
                    </ul>
                </div>
                <div class="endlineblock">
                    <ul>
                        <li class="endline" v-for="endline in endlines">
                            <div class="endline">{{endline}}</div>
                        </li>
                    </ul>
                </div>
                <div class="endfilling"></div>
                <div class="shopCart">
                  <transition name="fade">
                    <div class="text" @click="addCart($event)" v-show="!(getRiceTableMenu().count)"><Icon type="md-cart" /> {{ml.addtoshoppingcart}}</div>
                  </transition>
                </div>
                <cartcontrol :food="getRiceTableMenu()" v-show="getRiceTableMenu().count"></cartcontrol>
                <!-- <div class="cartcontrol-wrapper">
                  <cartcontrol :food="getRiceTableMenu(ricetable)"></cartcontrol>
                </div> -->
                </div>
        </div>
      </div>
    </div>
    <shopCart :seller="seller" :deliveryPrice="seller.deliveryPrice" :minPrice = "seller.minPrice" :selectFoods="selectFoods" :ml="ml" :data="data"></shopCart>
  </div>
</template>

<script>
import axios from 'axios'
import star from 'components/star/star'
import BScroll from 'better-scroll'
import cartcontrol from 'components/cartcontrol/cartcontrol'
import shopCart from 'components/shopCart/shopCart'

export default {
  components: {
    star: star,
    cartcontrol,
    shopCart
  },
  props: {
    data: {},
    ml: {},
    seller: {}
  },
  data() {
    return {
      title: this.data.monthmenu.title, // 'Menu van de maand',
      pricelines: this.data.monthmenu.pricelines, // ['Geldig tot 31/05/2021', '20 euro p.p. mini 2 personnen'],
      foodlines: this.data.monthmenu.foodlines, // ['kipsoep', 'mini loempia', 'peking eend'],
      endlines: this.data.monthmenu.endlines, // ['Bedankt voor uw bezoek']
      menuNr: this.data.monthmenu.menuNr
    }
  },
  mounted() {
    this.downloadMonthMenu()
  },
  watch: {
    '$route' (to, from) {
      if (to.path === '/monthmenu') {
        this.foodsScroll.refresh()
      }
    }
  },
  computed: {
    selectFoods() {
      let foods = []
      this.data.goods.forEach((good) => {
        good.foods.forEach((food) => {
          if (food.count) {
            foods.push(food)
          }
        })
      })
      return foods
    }
  },
  methods: {
    _initScroll() {
      console.log(`seller ${this.data.monthmenu.title},`)
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
    downloadMonthMenu() {
      let url = this.data.options.requestUrl + 'method=downloadmonthmenu&ln=' + this.data.options.ln + '&siterid=' + this.data.options.shopRid
      console.log('download monthmenu')
      console.log(url)
      axios.get(url).then((res) => {
        console.log('monthmenu downloaded')
        console.log(res)
        this.title = res.data.title;
        this.pricelines = res.data.pricelines;
        this.foodlines = res.data.foodlines;
        this.endlines = res.data.endlines;
        this.menuNr = res.data.menuNr;
        this.$nextTick(() => {
          this._initScroll(); // 初始化scroll
        })
      });
    },
    addCart(event) {
      if (!event._constructed) {
        return
      }
      this.$set(this.getRiceTableMenu(), 'count', 1)
      this.$root.eventHub.$emit('cart.add', event.target)
    },
    getRiceTableMenu() {
      let menuInDataJs = {}
      this.data.goods.forEach(menuGroup => {
        menuGroup.foods.forEach(menuItem => {
          if (menuItem.menunr === this.menuNr) {
            menuInDataJs = menuItem
          }
        });
      });
      // console.log(menuInDataJs)
      return menuInDataJs
    }
  }
}

</script>
