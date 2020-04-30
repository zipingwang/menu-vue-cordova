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
        <div class="column">
          <div class="block">
            <h1>{{ml.openinghour}}</h1>
            <ul class="list">
              <li class="item" v-for="line in seller.openinghour">{{line}}</li>
            </ul>
          </div>
        </div>      
        <div class="column">
          <div class="block">
            <h1>{{ml.telephone}}</h1>
            <ul class="list">
              <li class="item" v-for="tel in seller.telefoon">{{tel}}</li>
            </ul>
          </div>
        </div>       
        <div class="column">
          <div class="block">
            <h1>{{ml.address}}</h1>
            <ul class="list">
              <li class="item" v-for="line in seller.address">{{line}}</li>
            </ul>
          </div>
        </div>        
         
      </div>
      <div class="divider"></div>
      <div class="map">
        <iframe @load="alert('load')" src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d264.1796438045355!2d3.1150691671144806!3d50.95488935338752!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47c335ebf3ff4443%3A0xbccbcc65bdf9d37e!2sShanghai%20meeneemchinees!5e0!3m2!1snl!2sbe!4v1588192361967!5m2!1snl!2sbe" width="600" height="450" frameborder="0" style="border:0;" allowfullscreen="" aria-hidden="false" tabindex="0"></iframe>
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
