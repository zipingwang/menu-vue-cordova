import Vue from 'vue'
import App from './App'
import VueRouter from 'vue-router'
import goods from 'components/goods/goods'
import ratings from 'components/ratings/ratings'
import monthmenu from 'components/monthmenu/monthmenu'
import info from 'components/info/info'
import ricetable from 'components/ricetable/ricetable'
import seller from 'components/seller/seller'
import vueTap from 'v-tap'
import fastclick from 'fastclick'
import Vuex from 'vuex'
import VueLazyload from 'vue-lazyload'

Vue.use(vueTap)
Vue.use(VueRouter)
Vue.use(Vuex)
Vue.use(VueLazyload);

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
    path: '/seller',
    component: seller
  }],
  linkActiveClass: 'active'
})

new Vue({
  router,
  store,
  template: '<App/>',
  components: {
    App
  },
  data: {
    eventHub: new Vue()
  },
  created() {
    ml.trans = ml[`${ml.ln}`]
  }
}).$mount('#app')

// ml.trans = ml[`${ml.ln}`] /* can't put here, must in created, otherwise not accessed in App.vue */
// router.push(`${ml.activetab}`)
// console.log(JSON.stringify(data.seller))
// alert('default tab' + data.seller.defaultTab)
router.push(`${ml.visibletabs[data.seller.defaultTab]}`)
