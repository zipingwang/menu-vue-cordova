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
    <div class="tab-item" v-if="visibletabs.includes('goods')">
      <router-link to="/goods">{{ml.product}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('rating')">
      <router-link to="/ratings">{{ml.rating}}</router-link>
    </div>
    <div class="tab-item" v-if="visibletabs.includes('monthmenu')" >
      <router-link to="/monthmenu">{{ml.monthmenu}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('ricetable')">
      <router-link to="/ricetable">{{ml.ricetable}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('info')">
      <router-link to="/info">{{ml.info}}</router-link>
    </div>
    <div class="tab-item" v-if="visibletabs.includes('shop')">
      <router-link to="/seller">商家</router-link>
    </div>
  </div>
  <keep-alive>
      <router-view :seller="seller" :data="data" :ml="ml"></router-view>    
  </keep-alive>
</div>

</template>

<script>
import header from 'components/header/header'

const ERR_OK = 0

export default {
  data() {
    return {
      seller: data.seller,
      data: data,
      ml: ml.trans,
      visibletabs: ml.visibletabs,
      activetab: ml.activetab
    }
  },
  computed: {
    showgoodstab() {
      return ml.visibletabs.includes('goods');
    }
  },
  components: {
    'v-header': header
  }
}

</script>
