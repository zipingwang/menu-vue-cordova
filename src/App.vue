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
  <signalr ref="mysignalr" :seller="seller" :ml="trans" :data="data"></signalr>
  <v-header :seller="seller" :lns="lns" :ml="trans"></v-header>
  <div class="tab">
    <div class="tab-item" v-if="visibletabs.includes('goods')">
      <router-link to="/goods">{{trans.product}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('rating')">
      <router-link to="/ratings">{{trans.rating}}</router-link>
    </div>
    <div class="tab-item" v-if="visibletabs.includes('monthmenu')" >
      <router-link to="/monthmenu">{{trans.monthmenu}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('ricetable')">
      <router-link to="/ricetable">{{trans.ricetable}}</router-link>
    </div>
    <div class="tab-item"  v-if="visibletabs.includes('info')">
      <router-link to="/info">{{trans.info}}</router-link>
    </div>
    <div class="tab-item" v-if="visibletabs.includes('shop')">
      <router-link to="/seller">商家</router-link>
    </div>
    <div class="tab-item" v-if="data.options.cusId != ''">
      <router-link to="/admin">{{trans.mine}}</router-link>
    </div>
  </div>
  <keep-alive>
      <router-view :seller="seller" :data="data" :ml="trans"></router-view>
  </keep-alive>
</div>

</template>

<script>
import header from 'components/header/header'
import signalr from 'components/signalr/signalr'

const ERR_OK = 0

export default {
  data() {
    return {
      seller: data.seller,
      data: data,
      trans: ml.trans,
      lns: data.seller.lns,
      visibletabs: ml.visibletabs,
      activetab: ml.activetab,
      dataReday: false
    }
  },
  created() {
    this.data.currentlnindex = 0
    this.$root.eventHub.$on('ml.change', this.changeLanguage)
    this.$root.eventHub.$on('signalr.downloaded', this.menudownloaded)
  },
  computed: {
    urlVars() {
      var vars = {};
      var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&#]*)/gi, function(m, key, value) {
        vars[key] = value;
      });
      return vars;
    }
  },
  components: {
    'v-header': header,
    'signalr': signalr
  },
  methods: {
    changeLanguage(lan, index) {
      // alert('changeLanguage')
      // console.log(ml[lan])
      this.trans = ml[lan]
      this.data.currentlnindex = index
    },
    menudownloaded(data) {
      // alert('menudownloaded')
      var obj = JSON.parse(data)
      console.log(data)
    }
  }
}

</script>
