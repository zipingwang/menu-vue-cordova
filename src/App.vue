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
  <v-header :seller="seller" :lns="lns" :ml="trans" :data="data"></v-header>
  <div class="tab">
    <div class="tab-item" v-if="visibletabs.includes('goods') && data.goods.length>0">
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
    <!-- <div class="tab-item" v-if="data.options.changepasswordtoken !== ''">
      <router-link to="/resetPassword">change password</router-link>
    </div> -->
  </div>
  <keep-alive>
      <router-view :seller="seller" :data="data" :ml="trans"></router-view>
  </keep-alive>
</div>

</template>

<script>
import header from 'components/header/header'
import signalr from 'components/signalr/signalr'
import axios from 'axios'
import registerVue from './components/register/register.vue'

const ERR_OK = 0

export default {
  data() {
    return {
      seller: data.seller,
      data: data,
      trans: ml.trans,
      lns: data.seller.lns,
      visibletabs: ml.visibletabs,
      // activetab: ml.activetab,
      dataReday: false
    }
  },
  created() {
    document.title = this.seller.name
    this.data.currentlnindex = 0
    this.$root.eventHub.$on('ml.change', this.changeLanguage)
    this.$root.eventHub.$on('signalr.downloaded', this.menudownloaded)
    this.$root.eventHub.$on('login.loggedin', this.onLoggedIn)
    this.$root.eventHub.$on('login.loggedOut', this.onLoggedOut)
    this.$root.eventHub.$on('signalr.onSessionExpired', this.onSessionExpired)

    console.log('app created')
     // Active
    window.addEventListener('focus', this.onFocus);
    // Inactive
    window.addEventListener('blur', this.onBlur);
    if (data.options.shopId !== '') {
      this.downloadData()
    }
  },
  mounted() {
  },
  computed: {
    urlVars() {
      var vars = {};
      var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&#]*)/gi, function(m, key, value) {
        vars[key] = value;
      });
      return vars;
    },
    windowFocused() {
      return !document.hidden
    },
    shopDataUrl() {
      if (this.data.options.shopId) {
        return this.data.options.dataUrl + '?siteid=' + this.data.options.shopId
      } else {
        return '' // single site
        // return this.data.options.dataUrl + '?siteid=root'
      }
    }
  },
  watch: {
    windowFocused(newValue, oldValue) {
      log('windowFocused')
      log(newValue)
      if (newValue) {
        this.$refs.mysignalr.connectToSignalRServer()
      }
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
      console.log(lan)
      console.log(this.data)
      this.trans = ml[lan]
      this.data.currentlnindex = index
      this.data.options.ln = lan
      this.data.options.lnIndex = index
    },
    menudownloaded(data) {
      // alert('menudownloaded')
      var obj = JSON.parse(data)
      console.log(data)
    },
    onLoggedIn(user) {
      console.log('onLoggedIn in app')
      console.log(user)
      this.data.options.loggedIn = true
      this.data.options.cusId = user.rid
      this.data.options.isAdmin = user.isAdmin
      this.data.options.customerToken = user.customerToken
    },
    onSessionExpired(obj) {
      console.log('onSessionExpired in app')
      this.data.options.loggedIn = false
      this.data.options.cusId = ''
      this.data.options.isAdmin = ''
      this.data.options.customerToken = ''
    },
    onLoggedOut() {
      console.log('onLoggedOut in app')
      this.data.options.loggedIn = false
      this.data.options.cusId = ''
      this.data.options.isAdmin = false
    },
    onFocus() {
      console.log('onFocus')
      this.$refs.mysignalr.checkConnect()
    },
    onBlur() {
      console.log('onBlur')
    },
    downloadData() {
      axios.get(this.shopDataUrl).then((res) => {
        console.log('get from customerdaga.json')
        console.log(res)

        // res.data.options = {}
        // this.seller = res.data.seller
        console.log(this.data.options)
        // set it again to refresh data
        this.lns = res.data.seller.lns
        this.seller = res.data.seller
        this.data.goods = res.data.goods
        this.data.ricetables = res.data.ricetables
        this.data.options.shopRid = res.data.seller.sellerRid

        // document.title = this.seller.name

        if (this.data.options.takeaway === '1') {
          this.data.goods.forEach(menuGroup => {
            menuGroup.foods.forEach(menuItem => {
              menuItem.price = menuItem.takeawayPrice
            });
          })
        } else {
          this.data.goods.forEach(menuGroup => {
            menuGroup.foods.forEach(menuItem => {
              menuItem.price = menuItem.restaurantPrice
            });
          })
        }

        this.$root.eventHub.$emit('signalr.dataDownloaded')
      });
    }
  }
}

</script>
