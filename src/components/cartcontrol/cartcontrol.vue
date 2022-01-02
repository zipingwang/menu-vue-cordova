<template lang="html">

  <div class="cartcontrol">
    <transition name="fadeRotate">
      <div class="cart-decrease" v-show="food.count>0" @click.stop.prevent="decreaseCart()">
          <span class="icon-remove_circle_outline inner"></span>
      </div>
    </transition>
    <div class="cart-count" v-show="food.count>0">
      {{food.count}}
    </div>
    <div class="cart-add" @click.stop.prevent="addCart($event)">
      <i class="icon-add_circle"></i>
    </div>
  </div>

</template>

<script>
import Vue from 'vue'

export default {
  props: {
    food: Object,
    ml: Object
  },
  data() {
    return {
      prevClick: {}
    }
  },
  created() {
    this.prevClick = new Date()
  },
  methods: {
    addCart(event) {
      console.log('addCart')
      if (!event._constructed) {
        return
      }
      var preClickTemp = new Date(this.prevClick)
      this.prevClick = new Date()
      if (preClickTemp.setMilliseconds(preClickTemp.getMilliseconds() + 300) > new Date()) {
        console.log('skip')
        return
      }
      var allowed = checkTime(this.food.menunr, menuAllowedTimeSlots)
      if (!allowed) {
        this.$Modal.success({
          title: this.ml.info,
          content: this.ml.lunchboxnotallowedmsg,
          okText: this.ml.ok
        });
        return
      }
      allowed = checkDate(this.food.menunr, menuAllowedDateSlots)
      console.log('allowed')
      console.log(this.food.menunr)
      console.log(allowed)
      if (!allowed) {
        this.$Modal.success({
          title: this.ml.info,
          content: menuAllowedDateSlots.msg,
          okText: this.ml.ok
        });
        return
      }
      this.$root.eventHub.$emit('cart.add', this.food)
      this.$root.eventHub.$emit('cart.drop', event.target)
    },
    decreaseCart() {
      if (!this.food.count) {
        return
      }
      console.log('decreaseCart')
      var preClickTemp = new Date(this.prevClick)
      this.prevClick = new Date()
      if (preClickTemp.setMilliseconds(preClickTemp.getMilliseconds() + 300) > new Date()) {
        console.log('skip')
        return
      }

      this.$root.eventHub.$emit('cart.decrease', this.food)
      // this.food.count--;
    }
  }
}

</script>

<style lang="stylus">

.cartcontrol
  .cart-decrease
    display inline-block
    padding 6px
    transition: all .4s linear
    .inner
      line-height 24px
      font-size 24px
      color rgb(0,160,220)
      transition all 0.4s linear
    &.fadeRotate-enter-active, &.fadeRotate-leave-active
      transform translate3d(0,0,0)
      .inner
        display inline-block
        transform rotate(0)
    &.fadeRotate-enter, &.fadeRotate-leave-active
      opacity: 0
      transform translate3d(24px,0,0)
      .inner
        transform rotate(180deg)
  .cart-count
    display inline-block
    vertical-align top
    font-size 10px
    color rgb(147,153,159)
    line-height 24px
    text-align center
    padding 6px 0
  .cart-add
    display inline-block
    vertical-align top
    font-size 24px
    color rgb(0,160,220)
    line-height 24px
    padding 6px
</style>
