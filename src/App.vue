<style lang="stylus" rel="stylesheet/stylus">
  @import 'common/stylus/index'
  .tab
    display:flex
    width:100%
    height:40px
    line-height:40px
    border-1px(rgba(7,17,27,0.1))
    .tab-item
      flex:1
      text-align:center
      a
        display:block
        font-size:14px
        color rgb(77,85,93)
        &.active
          font-size 14px
          color rgb(240,20,20)
</style>

<template>
<div>
  <v-header :seller="seller"></v-header>
  <div class="tab">
    <div class="tab-item">
      <router-link to="/goods">商品</router-link>
    </div>
    <div class="tab-item">
      <router-link to="/ratings">评论</router-link>
    </div>
    <div class="tab-item">
      <router-link to="/promotion">Promotion</router-link>
    </div>
    <div class="tab-item">
      <router-link to="/info">Info</router-link>
    </div>
    <div class="tab-item">
      <router-link to="/seller">商家</router-link>
    </div>
  </div>
  <keep-alive>
    <router-view :seller="seller" :data="data"></router-view>
  </keep-alive>
</div>

</template>

<script>
import header from 'components/header/header'
import axios from 'axios'

const ERR_OK = 0

export default {
  data() {
    return {
      seller: {},
      data: {}
    }
  },
  created() {
    axios.get('static/data.json').then((res) => {
      this.seller = res.data.seller
      this.data = res.data
      console.log(`get datajson from server in appvue ${JSON.stringify(res.data)},`)
      console.log(`get promotion in appvue ${res.data.promotion.title},`)
    })
  },
  components: {
    'v-header': header
  }
}

</script>
