<template lang="html">

  <div class="goods">
    <div class="menu-wrapper" ref="menuWrapper">
      <ul>
        <li v-for="(item,index) in data.goods" @click="menuClick(index,$event)" :class="index==menuCurrentIndex?'menu-item-selected':'menu-item'">
          <span class="text">
            <!-- <iconMap v-show="item.type>0" :iconType="item.type"></iconMap> -->
            {{item.name[data.currentlnindex]}}
          </span>
        </li>
      </ul>
    </div>
    <div class="foods-wrapper" id="wrapper" ref="foodsWrapper">
      <ul>
        <li v-for="item in data.goods" class="food-list food-list-hook">
          <h1>{{item.name[data.currentlnindex].replace("- ", "")}}</h1>
          <ul>
            <li v-for="food in item.foods" class="food-item" @click="goDetail(food)">
              <div class="icon">
                <img width="57" height="57" v-lazy="food.icon"/>
              </div>
              <div class="content">
                <!-- <span class="food">{{food.name}}</span> <span class="nr">nr. {{food.menunr}}</span>  -->
                <h2>{{food.name[data.currentlnindex]}}</h2>
                <!-- <p class="description" v-show="food.description">{{food.description}}</p>                  -->
                <!-- <p class="description" v-show="food.menunr">Nr. {{food.menunr}}</p> -->
                <div class="price">
                  <span class="newPrice"><span class="unit">€ </span>{{food.price}}</span>
                  <span v-show="food.oldPrice" class="oldPrice">€{{food.oldPrice}}</span>
                  <span class="menunr" v-show="food.menunr">Nr. {{food.menunr}}</span>
                </div>
                <div class="sell-info">
                  <div class="star-wrapper" v-if="food.rating">
                    <star :size="24" :score="food.rating"></star>
                  </div>
                  <!-- <span class="sellCount">月售{{food.sellCount}}份</span> -->
                  <!-- <span class="rating">好评率{{food.rating}}%</span> -->
                </div>
                <div class="cartcontrol-wrapper">
                  <cartcontrol :food="food"></cartcontrol>
                </div>
              </div>
            </li>
          </ul>
        </li>
      </ul>
    </div>
    <shopCart ref="myShopCart" :seller="seller" :deliveryPrice="seller.deliveryPrice" :minPrice = "seller.minPrice" :selectFoods="selectFoods" :ml="ml" :data="data"></shopCart>
    <foodDetail :food="selectedFood" :data="data" v-if="selectedFood" :ml="ml" ref="myFood"></foodDetail>
  </div>

</template>

<script>
import iconMap from 'components/iconMap/iconMap'
import BScroll from 'better-scroll'
import shopCart from 'components/shopCart/shopCart'
import cartcontrol from 'components/cartcontrol/cartcontrol'
import foodDetail from 'components/foodDetail/foodDetail'
import axios from 'axios'
import Vue from 'vue'
import star from 'components/star/star'

const ERR_OK = 0
const eventHub = new Vue()
export default {
  props: {
    seller: Object,
    ml: Object,
    data: {}
  },
  created() {
    console.log('created in goods.vue')
    // setTimeout(() => {
    //   this._initScroll(); // 初始化scroll
    //   this._calculateHeight(); // 初始化列表高度列表
    // }, 500)
    // this.$nextTick(() => {
    //   this._initScroll(); // 初始化scroll
    //   this._calculateHeight(); // 初始化列表高度列表
    // })
    this.$root.eventHub.$on('signalr.onOrderConfirmedFromServerToWeb', this.onOrderConfirmedFromServerToWeb)
    this.$root.eventHub.$on('signalr.dataDownloaded', this.onDatDownLoaded)
  },
  mounted() {
    // this.$nextTick(() => {
    //   this._initScroll(); // 初始化scroll
    //   this._calculateHeight(); // 初始化列表高度列表
    // })
  },
  watch: {
    '$route' (to, from) {
      if (to.path === '/goods') {
        this.foodsScroll.refresh()
        this.$refs.myShopCart.closeChild()
        if (this.$refs.myFood !== undefined) {
          this.$refs.myFood.hide()
        }
      }
    }
  },
  data() {
    return {
      // data: data,
      // goods: data.goods, /* don't create own property, it will breke the change change. change in parant data.goods will not go through child */
      listHeight: [],
      foodsScrollY: 0,
      selectedFood: ''
    }
  },
  computed: {
    menuCurrentIndex() {
      for (let i = 0, l = this.listHeight.length; i < l; i++) {
        let topHeight = this.listHeight[i]
        let bottomHeight = this.listHeight[i + 1]
        if ((this.foodsScrollY + this.$refs.menuWrapper.clientHeight) > this.listHeight[this.listHeight.length - 2]) {
          console.log(this.listHeight.length - 1)
          return this.listHeight.length - 2
        } else if (!bottomHeight || (this.foodsScrollY >= topHeight && this.foodsScrollY < bottomHeight)) {
          return i
        }
      }
      return 0
    },
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
      let smallScreen = screen.width <= 800;
      console.log(`screen width ${screen.width}, smallScreen ${smallScreen}`)
      this.menuGroupScroll = new BScroll(this.$refs.menuWrapper, {
        click: true
      });
      this.foodsScroll = new BScroll(this.$refs.foodsWrapper, {
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
      // 监控滚动事件
      this.foodsScroll.on('scroll', (pos) => {
        this.foodsScrollY = Math.abs(Math.round(pos.y))
        let menuGroupHeight = 54
        let offset = this.$refs.menuWrapper.clientHeight - (this.menuCurrentIndex + 1) * menuGroupHeight
        if (offset < 0) {
          if ((this.foodsScrollY + this.$refs.menuWrapper.clientHeight) > this.listHeight[this.listHeight.length - 2]) {
            this.menuGroupScroll.scrollTo(0, this.$refs.menuWrapper.clientHeight - (this.data.goods.length * menuGroupHeight), 300)
          } else {
            this.menuGroupScroll.scrollTo(0, offset, 300)
          }
        } else {
          this.menuGroupScroll.scrollTo(0, 0, 300)
        }
      })
    },
    _calculateHeight() {
      let foodList = this.$refs.foodsWrapper.querySelectorAll('.food-list-hook')
      let height = 0
      this.listHeight.push(height)
      for (let i = 0, l = foodList.length; i < l; i++) {
        let item = foodList[i]
        height += item.clientHeight
        this.listHeight.push(height)
      }
    },
    menuClick(index, event) {
      if (!event._constructed) {
        return
      }
      this.foodsScroll.scrollTo(0, -this.listHeight[index], 300)
    },
    goDetail(food) {
      this.selectedFood = food
      if (food.image) {
        this.$nextTick(() => {
          this.$refs.myFood.show()
        })
      }
    },
    onOrderConfirmedFromServerToWeb(order, addremove) {
      if (addremove !== '1') {
        return
      }
      this.selectFoods.length = 0
      this.data.goods.forEach((good) => {
        good.foods.forEach((food) => {
          if (food.count) {
            food.count = 0
          }
        })
      })
    },
    onDatDownLoaded() {
      console.log('onDataDownLoaded')
      this.$nextTick(() => {
        this._initScroll(); // 初始化scroll
        this._calculateHeight(); // 初始化列表高度列表
      })
    }
  },
  components: {
    iconMap,
    shopCart,
    cartcontrol,
    foodDetail,
    star
  }
}

</script>

<style lang="stylus">
@import '../../common/stylus/mixin'
  .goods
    display flex
    position absolute
    top 174px
    bottom 46px
    width 100%
    overflow hidden
    .menu-wrapper
      flex 0 0 80px
      width 80px
      background #f3f5f7
      margin-top: 2px;
      color rgb(74, 62, 52)
      font-weight 100
      overflow hidden
      .menu-item-selected
        // background rgb(86, 77, 71)
        background rgb(96, 80, 69)
        // background rgb(117, 120, 113)
        color white
        margin-top -1px
      .menu-item,.menu-item-selected
        position relative
        display table
        height 54px
        line-height 14px
        width 80px
        padding 0 12px
        &:last-child:after
          content none
      .menu-item:after
          position: absolute
          content: ''
          left: 12px
          width: 56px
          bottom: 0
          border-bottom: 1px solid rgba(7,17,27,0.1)
        .text
          display table-cell
          vertical-align middle
          font-size 12px
          font-weight 500
          white-space normal
          line-height 14px
          .iconMap
            vertical-align middle
    .foods-wrapper
      flex 1
      margin-top: 2px;
      .food-list
        h1
          height 26px
          line-height 26px
          padding-left 12px
          font-size 12px
          color rgb(74, 62, 52)
          // color rgb(147,153,159)
          background #f3f5f7
          border-left 2px solid #d9dde1
          text-align center
      .food-item
        position relative
        display flex
        margin: 0 18px;
        padding: 18px 0;
        border-bottom 1px solid rgba(7,17,27,0.1)
        .icon
          flex 0 0 57px
        &:last-child
          border-bottom none
        .content
          flex 1
          padding-left 10px
          .food
            margin 2px 0 8px 0
            font-size 14px
            line-height 14px
            height 14px
            font-weight 700
            color rgb(7,17,27)
          .nr
            margin 2px 0 8px 5px
            font-size 12px
            line-height 12px
            height 14px
            font-style italic
            font-weight 700
            color rgb(147,153,159)
          h2
            margin 2px 0 8px 0
            font-size 14px
            line-height 14px
            height 14px
            font-weight 700
            color rgb(7,17,27)
          .sell-info,.description
            font-size 10px
            color rgb(147,153,159)
            line-height 10px
            .star-wrapper
              margin-right 4px
            .sellCount
              margin-right 4px
          .description
            font-size 10px
            margin-bottom 8px
            line-height: 12px
          .price
            margin-top 20px
            font-size 10px
            font-weight 700
            line-height 24px
            .newPrice
              font-size 14px
              color rgb(240,20,20)
              .unit
                font-size 10px
                font-weight normal
            .oldPrice
              text-decoration line-through
              color rgb(147,153,159)
              padding-left 4px
            .menunr
              margin-left 10px
          .cartcontrol-wrapper
            position: absolute
            right: 0
            bottom 20px
            z-index 20

</style>
