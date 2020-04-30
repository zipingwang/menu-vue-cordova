<style lang="stylus" scoped>
.ratingsWrapper
  position: absolute
  top: 174px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden
.ratings-content
  width 100%
  text-align center
  color rgb(77,85,93)
  font-size 36px
  line-height 26px
  .title
    font-size 24px
    line-height 24px   
    margin-top 20px
  .pricelineblock      
    .priceline
      font-size 16px
      line-height 16px   
      margin-top 20px
  .foodblock
    margin-top 50px
    .foodlineblock        
      .foodline
        font-size 16px
        height 16px        
      .seperator
        margin-top 5px
        height 30px
        font-size 16px
        line-height 30px       
  .endlineblock
    .endline
      margin-top 50px
      font-size 16px
      height 30px
      line-height 30px
    
</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper">
    <div class="ratings-content">
      <div>
        <div class="title">
            {{title}}
          </div>
          <div class="pricelineblock">
              <ul>
                  <li class="priceline" v-for="priceline in pricelines">
                      <div class="priceline">{{priceline}}</div>                   
                  </li>
              </ul>
          </div>
          <div class="foodblock">
              <ul>
                  <li class="foodlineblock" v-for="(food, index) in foodlines">
                      <div class="foodline">{{food}}</div>
                      <div class="seperator" v-if="index != foodlines.length - 1">***</div>
                  </li>
              </ul>
          </div>
          <div class="endlineblock">
              <ul>
                  <li class="endline" v-for="endline in endlines">
                      <div class="endline">{{endline}}</div>                   
                  </li>
              </ul>
          </div> 
        </div>     
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import star from 'components/star/star'
import BScroll from 'better-scroll'

export default {
  components: {
    star: star
  },
  props: {
    data: {}
  },
  data() {
    return {
      title: this.data.promotion.title, // 'Menu van de maand',
      pricelines: this.data.promotion.pricelines, // ['Geldig tot 31/05/2021', '20 euro p.p. mini 2 personnen'],
      foodlines: this.data.promotion.foodlines, // ['kipsoep', 'mini loempia', 'peking eend'],
      endlines: this.data.promotion.endlines // ['Bedankt voor uw bezoek']
    }
  },
  created() {
    axios.get('static/data.json').then((res) => {
      this.goods = res.data.goods;
      this.$nextTick(() => {
        this._initScroll(); // 初始化scroll
      })
    });
  },
  methods: {
    _initScroll() {
      console.log(`seller ${this.data.promotion.title},`)
      let smallScreen = screen.width <= 800;
      console.log(`screen width ${screen.width}, smallScreen ${smallScreen}`)
      this.foodsScroll = new BScroll(this.$refs.ratingsWrapper, {
        click: true,
        probeType: 3,
        scrollbar: {
          fade: smallScreen,
          interactive: !smallScreen // new in 1.8.0
        },
        mouseWheel: {
          speed: 20,
          invert: false
        }
      });
    }
  }
}

</script>
