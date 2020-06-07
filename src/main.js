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
import ViewUI from 'view-design';
import 'view-design/dist/styles/iview.css';
// import VueI18n from 'vue-i18n';
import en from 'view-design/dist/locale/en-US';
import nl from 'view-design/dist/locale/nl-NL';
import zh from 'view-design/dist/locale/zh-CN';

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
    data.options.tableNr = ''
    data.options.ln = ''
    data.options.takeaway = '0'
    data.options.cusId = ''
    if ('table' in urlVars) {
      data.options.tableNr = urlVars['table']
    }
    if ('ln' in urlVars) {
      data.options.ln = urlVars['ln']
      ml.trans = ml[data.options.ln] /* dynamic inject */
    }
    if ('takeaway' in urlVars) {
      data.options.takeaway = urlVars['takeaway']
    }
    console.log(this.getUrlVars())
    console.log(urlVars)
    // window.location.href = window.location.href.substring(0, window.location.href.indexOf('?'))
    alert(window.location.href)
  },
  methods: {
    getUrlVars() {
      var vars = {};
      var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
        vars[key] = value;
      });
      return vars;
    }
  }
}).$mount('#app')

// ml.trans = ml[`${ml.ln}`] /* can't put here, must in created, otherwise not accessed in App.vue */
// router.push(`${ml.activetab}`)
// console.log(JSON.stringify(data.seller))
// alert('default tab' + data.seller.defaultTab)
router.push(`${ml.visibletabs[data.seller.defaultTab]}`)
