<template lang="html">
  <div>
  <transition name="fade">
    <div v-show="showDetail" class="detail" @click="hide()">
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="showDetail">
      <div class="foodDetail">
        <div class="back" @click="hide()">
          <i class="icon-arrow_lift"></i>
        </div>
        <img :src="food.image" :height="imgHeight" width="100%" onerror="this.onerror=null; this.src='static/img/nomenupicture.jpg'">
        <div class="info">
          <div class="title">{{food.name[data.currentlnindex]}}</div>
          <p class="desc" v-show="food.menunr">Nr. {{food.menunr}}</p>
          <div class="star-wrapper" v-if="food.rating">
            <star :size="24" :score="food.rating"></star>
          </div>
          <!-- <div class="desc">
            <span>月售{{food.sellCount}}</span>
            <span>好评率{{food.rating}}%</span>
          </div> -->
          <div class="price">
            <span class="unit">€</span>{{food.price}}
            <span class="oldPrice" v-show="food.oldPrice">€{{food.oldPrice}}</span>
          </div>
          <div class="shopCart">
            <transition name="fade">
              <div class="text" @click="addCart($event)" v-show="!food.count"><Icon type="md-cart" /> {{ml.addtoshoppingcart}}</div>
            </transition>
          </div>
          <cartcontrol :food="food" v-show="food.count"></cartcontrol>
        </div>
        <div class="divider"></div>
        <div class="desc">
          <div class="title">{{ml.description}}</div>
          <div class="content">{{food.info}}</div>
        </div>
        <!-- <div class="divider"></div>
        <div class="evaluation">
          <div class="title">
            商品评价
          </div>
          <div class="classify">
            <span v-for="(item,index) in classifyArr" class="item" :class="{'active':item.active,'bad':index==2,'badActive':item.active&&index==2}" @click="filterEvel(item)">
              {{item.name}}<span class="count">{{item.count}}</span>
            </span>
          </div>
          <div class="switch" @click="evelflag=!evelflag">
            <span class="icon-check_circle" :class="{'on':evelflag}"></span>
            <span class="text">只看有内容的评价</span>
          </div>
          <div class="evel-list">
            <ul>
              <li class="evel" v-for="evel in evelArr">
                <div class="userInfo">
                  <div class="time">{{evel.rateTime | time}}</div>
                  <div class="user">
                    <span>{{evel.username}}</span>
                    <span class="avatar"><img :src="evel.avatar" width="12" height="12"></span>
                  </div>
                </div>
                <div class="content">
                  <span class="icon" :class="evel.rateType?'icon-thumb_down':'icon-thumb_up'"></span>
                  <span class="text">{{evel.text}}</span>
                </div>
              </li>
            </ul>
          </div>
        </div> -->
      </div>
    </div>
  </transition>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'
import cartcontrol from 'components/cartcontrol/cartcontrol'
import star from 'components/star/star'

export default {
  components: {
    cartcontrol,
    star
  },
  props: {
    food: Object,
    data: {},
    ml: {}
  },
  data() {
    return {
      showDetail: false,
      currentCommmentCatetory: 0,
      classifyArr: [{
        name: '全部',
        count: this.food.ratings.length,
        commmentCatetory: 0,
        active: true
      }, {
        name: '推荐',
        count: this.food.ratings.filter((data) => data.rateType === 0).length,
        commmentCatetory: 1,
        active: false
      }, {
        name: '吐槽',
        count: this.food.ratings.filter((data) => data.rateType).length,
        commmentCatetory: 2,
        active: false
      }],
      evelflag: true
    }
  },
  computed: {
    evelArr() {
      if (this.detailWrapper) {
        this.$nextTick(() => {
          this.detailWrapper.refresh()
        })
      }
      return this.food.ratings.filter((data) => this.evelflag ? this.currentCommmentCatetory && data.text : this.currentCommmentCatetory)
    },
    imgHeight() {
      if (screen.width < 800) {
        return 350
      } else {
        return 450
      }
    }
  },
  methods: {
    show() {
      this.showDetail = true
      this.$nextTick(() => {
        this._initScroll()
      })
    },
    hide() {
      this.showDetail = false
    },
    // showToggle() {
    //   this.showDetail = !this.showDetail
    //   if (this.showDetail) {
    //     this.$nextTick(() => {
    //       this._initScroll()
    //     })
    //   }
    // },
    _initScroll() {
      if (!this.detailWrapper) {
        this.detailWrapper = new BScroll(this.$refs.detailWrapper, {
          click: true
        });
      } else {
        this.detailWrapper.refresh()
      }
    },
    addCart(event) {
      if (!event._constructed) {
        return
      }
      this.$set(this.food, 'count', 1)
      this.$root.eventHub.$emit('cart.add', event.target)
    },
    filterEvel(item) {
      this.currentCommmentCatetory = item.commmentCatetory
      this.classifyArr.forEach((data) => {
        data.active = false
      })
      item.active = true
    }
  }
}

</script>

<style lang="stylus" scoped>
// @media screen and (min-width: 800px)
  .detail
      position fixed
      left 0
      top 0
      bottom 48px
      width 100%
      z-index 30
      background rgba(7,17,27,0.8)
      backdrop-filter blur(10px)
      transition all 0.4s ease
.detailWrapper
  position fixed
  left 0
  top 0
  bottom 48px
  width 100%
  z-index 30
  background white
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
    transition all 0.4s ease
    &.move-enter-avtive,&.move-leave{
      transform translate3d(0,0,0)
    }
    &.move-enter,&.move-leave-active{
      transform translate3d(10%,0,0)
    }
.foodDetail
  .back
    position absolute
    color white
    top 12px
    left 6px
    font-size 20px
    padding 10px
    background-color rgba(7,17,27,0.2)
    border-radius 5px
  .info
    position relative
    box-sizing border-box
    width 100%
    padding 18px
    .star-wrapper
      margin-top 4px
    .title
      font-size 14px
      font-weight 700
      color rgb(7,17,27)
      line-height 14px
    .desc
      display flex
      padding 0
      padding-top 8px
      font-size 10px
      color rgb(147,153,159)
      line-height 10px
      span:last-child
        padding-left 12px
    .price
      display flex
      padding-top 18px
      font-size 14px
      font-weight 700
      color rgb(240,20,20)
      line-height 24px
      .unit
        font-size 10px
        font-weight normal
      .oldPrice
        padding-left 12px
        font-size 10px
        font-weight normal
        color rgb(147,153,159)
        line-height 24px
    .shopCart
      position absolute
      right 18px
      bottom 18px
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
      bottom 12px
  .desc
    padding 18px
    .title
      font-size 14px
      font-weight 500
      color #07111b
      margin-bottom 6px
    .content
      font-size 12px
      font-weight 200
      color rgb(77,85,93)
      line-height 24px
      padding 0 8px
  .evaluation
    padding 18px 0
    position relative
    .title
      padding-left 18px
      font-size: 14px
      font-weight 500
      color: #07111b
    .classify
      padding 18px 0
      margin 0 18px
      border-bottom 1px solid rgba(7,17,27,0.1)
      .item
        display inline-block
        font-size 12px
        padding 8px 12px
        line-height 16px
        background rgba(0,160,220,0.2)
        color rgb(77,85,95)
        margin-right 8px
        .count
          font-size 8px
          padding-left 2px
        &.active
          color white
          background rgb(0,169,220)
        &.bad
          background rgba(77,85,93,0.2)
        &.badActive
          background #4d555d
    .switch
      font-size 12px
      width 100%
      padding 12px 0 12px 18px
      color rgb(147,153,159)
      border-bottom 1px solid rgba(7,17,27,0.1)
      .icon-check_circle
        font-size 24px
        vertical-align middle
        &.on
          color #00c850
    .evel-list
      margin 0 18px
      .evel
        padding 16px 0
        border-bottom 1px solid rgba(7,17,27,0.1)
        .userInfo
          display flex
          color rgb(147,153,159)
          font-size 10px
          line-height 12px
          .time
            flex 1
          .user
            flex 1
            text-align right
            .avatar
              img
                padding-left 6px
                border-radius 50%
        .content
          padding-top 6px
          .icon
            font-size 12px
            line-height 24px
            &.icon-thumb_up
              color rgb(0,160,220)
            &.icon-thumb_down
              color rgb(147,153,159)
          .text
            font-size 12px
            color rgb(7,17,27)
            line-height 16px
            padding-left 4px

</style>
