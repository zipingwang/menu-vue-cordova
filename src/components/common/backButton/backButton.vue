<template lang="html">
    <div class="back" @click="click()">
         <i class="icon-arrow_lift"></i>
    </div>
</template>

<script>

export default {
  props: {
  },
  data() {
    return {
    }
  },
  methods: {
    click() {
      this.$emit('click')
    }
  }
}
</script>

<style lang="stylus" scoped>
.back
    // position absolute
    color rgb(77,85,93)
    top 12px
    left 6px
    font-size 20px
    padding 10px
</style>
