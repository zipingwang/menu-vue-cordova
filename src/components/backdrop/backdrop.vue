<template lang="html">
  <transition name="fade-backdrop">
    <div ref="myBackDrop" class="backdrop" v-show="isShow" @click="click()"></div>
  </transition>
</template>

<script>
export default {
  props: {
    isShow: Boolean,
    background: ''
  },
  mounted() {
    if (this.background !== '') {
      this.$refs.myBackDrop.style.backgroundColor = this.background
      /* caller pass parameter like this */
      /* background="rgba(7,17,27,0.1)" or :background="'rgba(7,17,27,0.1)'" */
    }
  },
  methods: {
    click() {
      this.$emit('click')
    }
  }
}

</script>

<style lang="stylus">
.backdrop
  position fixed
  top 0
  bottom 0
  left 0
  right 0
  background rgba(7,17,27,0.6)
  backdrop-filter blur(10px)
  z-index 40
  &.fade-backdrop-enter-active,&.fade-backdrop-leave-active
    transition opacity 0.5s
  &.fade-backdrop-enter,&.fade-backdrop-leave-active
    opacity 0
</style>
