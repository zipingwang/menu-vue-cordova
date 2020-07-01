import Vue from 'vue'
import App from './App'
import VueRouter from 'vue-router'
import goods from 'components/goods/goods'
import ratings from 'components/ratings/ratings'
import monthmenu from 'components/monthmenu/monthmenu'
import info from 'components/info/info'
import ricetable from 'components/ricetable/ricetable'
import seller from 'components/seller/seller'
import admin from 'components/admin/admin'
import vueTap from 'v-tap'
import fastclick from 'fastclick'
import Vuex from 'vuex'
import VueLazyload from 'vue-lazyload'
import ViewUI from 'view-design';
import 'view-design/dist/styles/iview.css';
// import VueI18n from 'vue-i18n';


Vue.use(vueTap)
Vue.use(VueRouter)
Vue.use(Vuex)
Vue.use(VueLazyload);
Vue.use(ViewUI);

const store = new Vuex.Store({
  state: {
    count: 0
  },
  // 添加的商品元素
  addCartEl: {},
  mutations: {
    increment(state) {
      state.count++
    }
  }
})
const router = new VueRouter({
  routes: [{
    path: '/goods',
    component: goods
  }, {
    path: '/ratings',
    component: ratings
  }, {
    path: '/monthmenu',
    component: monthmenu
  }, {
    path: '/info',
    component: info
  }, {
    path: '/ricetable',
    component: ricetable
  }, {
    path: '/admin',
    component: admin
  }, {
    path: '/seller',
    component: seller
  }],
  linkActiveClass: 'active'
})

// const i18n = new VueI18n({
//   locale: 'nl',  // set locale
//   messages  // set locale messages
// });

new Vue({
  router,
  store,
  // i18n,
  template: '<App/>',
  components: {
    App
  },
  data: {
    eventHub: new Vue()
  },
  created() {
    ml.trans = ml[`${ml.ln}`] /* dynamic inject */
    // var urlVars = this.getUrlVars()
    var urlVars = this.getUrlVars()
    data.options = {}
    data.options.client = 'vue'
    data.options.table = ''
    data.options.ln = ''
    data.options.takeaway = '1'
    data.options.cusId = '' /* 202 is admin */
    data.options.shopId = ''
    data.options.isAdmin = '0'
    data.options.loggedIn = false
    let baseUrl = this.getBaseUrl()
    data.options.baseUrl = baseUrl
    if (data.seller.sellerurl !== '' && data.seller.sellerurl.length > 0) {
      data.options.signalrUrl = data.seller.sellerurl + '/signalr'
      data.options.uploadUrl = data.seller.sellerurl + '/RequestHandler.ashx?'
    } else {
      data.options.signalrUrl = baseUrl + '/signalr'
      data.options.uploadUrl = baseUrl + '/RequestHandler.ashx?'
    }
    if ('table' in urlVars) {
      data.options.table = urlVars['table']
      data.options.takeaway = '0'
      console.log('table')
    }
    if ('ln' in urlVars) {
      data.options.ln = urlVars['ln']
      ml.trans = ml[data.options.ln] /* dynamic inject */
    }
    if ('takeaway' in urlVars) {
      data.options.takeaway = urlVars['takeaway']
      data.options.takeaway = '1'
    }
    if ('shopid' in urlVars) {
      data.options.shopId = urlVars['shopid']
    }
    if (data.options.takeaway === '1') {
      data.goods.forEach(menuGroup => {
        menuGroup.foods.forEach(menuItem => {
          menuItem.price = menuItem.takeawayPrice
        });
      })
    } else {
      data.goods.forEach(menuGroup => {
        menuGroup.foods.forEach(menuItem => {
          menuItem.price = menuItem.restaurantPrice
        });
      })
    }
    console.log(this.getUrlVars())
    console.log(urlVars)
    console.log(data.options)
  },
  methods: {
    getUrlVars() {
      var vars = {};
      var url = window.location.href
      var indexhash = window.location.href.indexOf('#')
      if (indexhash > 0) {
        url = window.location.href.substring(0, indexhash)
      }
      var parts = url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
        vars[key] = value;
      });
      return vars;
    },
    getBaseUrl() {
      let url = window.location.href
      let indexhash = window.location.href.indexOf('?')
      if (indexhash > 0) {
        url = window.location.href.substring(0, indexhash)
      } else {
        indexhash = window.location.href.indexOf('#')
        if (indexhash > 0) {
          url = window.location.href.substring(0, indexhash)
        }
      }
      return url
    }
  }
}).$mount('#app')

// ml.trans = ml[`${ml.ln}`] /* can't put here, must in created, otherwise not accessed in App.vue */
// router.push(`${ml.activetab}`)
// console.log(JSON.stringify(data.seller))
// alert('default tab' + data.seller.defaultTab)
router.push(`${ml.visibletabs[data.seller.defaultTab]}`)
// router.push(`admin`)
