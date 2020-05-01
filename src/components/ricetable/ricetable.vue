<style lang="stylus" scoped>
* {
  box-sizing: border-box;
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: wrap;
}

/* Create four equal columns */
.column {
  flex: 25%;
  padding: 20px;
}

/* On screens that are 992px wide or less, go from four columns to two columns */
@media screen and (max-width: 992px) {
  .column {
    flex: 50%;
  }
}

/* On screens that are 600px wide or less, make the columns stack on top of each other instead of next to each other */
@media screen and (max-width: 600px) {
  .row {
    flex-direction: column;
  }
}

.ratingsWrapper
  position: absolute
  top: 174px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden
.ratings-content
  .row
    .column 
      .block
        h1
          // margin 0 18px
          padding 0px 0 12px 0
          border-bottom 1px solid rgba(7,17,27,0.1)
        .list
          .item
            font-size 12px
            // font-weight 200
            color rgb(7,17,27)
            line-height 10px
            padding 10px 10px
            border-bottom 1px solid rgba(7,17,27,0.1)
            &:last-child
              border none
  .map
    text-align center
    margin-top 20px
    margin-bottom 100px
    
</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper">
    <div class="ratings-content">
      <div class="divider"></div>
      <div class="row">
        <div class="column" v-for="ricetable in ricetables">
           <div>
                <div class="title">
                    {{ricetable.title}}
                </div>
                <div class="pricelineblock">
                    <ul>
                        <li class="priceline" v-for="priceline in ricetable.pricelines">
                            <div class="priceline">{{priceline}}</div>                   
                        </li>
                    </ul>
                </div>
                <div class="foodblock">
                    <ul>
                        <li class="foodlineblock" v-for="(food, index) in ricetable.foodlines">
                            <div class="foodline">{{food}}</div>
                            <div class="seperator" v-if="index != ricetable.foodlines.length - 1">***</div>
                        </li>
                    </ul>
                </div>
                <div class="endlineblock">
                    <ul>
                        <li class="endline" v-for="endline in ricetable.endlines">
                            <div class="endline">{{endline}}</div>                   
                        </li>
                    </ul>
                </div> 
            </div>     
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
    data: {},
    ml: {},
    seller: {}
  },
  data() {
    return {
      ricetables: this.data.ricetables,
      title: this.data.ricetables.title, // 'Menu van de maand',
      pricelines: this.data.promotion.pricelines, // ['Geldig tot 31/05/2021', '20 euro p.p. mini 2 personnen'],
      foodlines: this.data.promotion.foodlines, // ['kipsoep', 'mini loempia', 'peking eend'],
      endlines: this.data.promotion.endlines // ['Bedankt voor uw bezoek']
    }
  },
  mounted() {
    setTimeout(() => {
      this._initScroll()
    }, 3000);
  },
  methods: {
    _initScroll() {
      // alert('ok')
      console.log(`_initScrollseller ${this.data.promotion.title},`)
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
