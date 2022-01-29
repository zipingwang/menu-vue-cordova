<template lang="html">
    <div class="wrapper">
         <div class="shopCart">
            <transition name="fade">
              <div class="text" @click="addCart($event)" v-show="!food.count"><Icon type="md-cart" /> {{ml.addtoshoppingcart}}</div>
            </transition>
          </div>
          <cartcontrol :food="food" :ml="ml" v-show="food.count"></cartcontrol>
    </div>
</template>

<script>

import cartcontrol from 'components/common/cartcontrol/cartcontrol'

export default {
  components: {
    cartcontrol
  },
  props: {
    food: {},
    ml: {}
  },
  data() {
    return {
      prevClick: new Date()
    }
  },
  methods: {
    addCart(event) {
      var preClickTemp = new Date(this.prevClick)
      this.prevClick = new Date()
      if (preClickTemp.setMilliseconds(preClickTemp.getMilliseconds() + 300) > new Date()) {
        console.log('skip')
        return
      }

      if (!event._constructed) {
        return
      }
      this.$root.eventHub.$emit('cart.add', this.food)
      this.$root.eventHub.$emit('cart.drop', event.target)
    }
  }
}
</script>

<style lang="stylus" scoped>
.wrapper
  // position relative
  .shopCart
        position absolute
        top 3px
        height 24px
        text-align center
        .text
          box-sizing border-box
          height 100%
          line-height 24px
          color white
          font-size 10px
          padding 0 12px
          border-radius 12px
          background rgb(0,160,220)
          // &.fade-enter-active,&.fade-leave-active{
          //   transition opacity .2s
          // }
          // &.fade-enter,&.fade-leave-active{
          //   opacity 0
          // }
      .cartcontrol
        position absolute
        height 24px
        top 0px
</style>
