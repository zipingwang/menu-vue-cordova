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
      <div class="map" :height="mapheight + 10">
        <iframe :src="seller.googlemapurl" :width="mapwidth" :height="mapheight" frameborder="0" style="border:0;" allowfullscreen="" aria-hidden="false" tabindex="0"></iframe>
      </div>
      <div height="10px"></div>
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
      address: this.seller.address,
      telefoon: this.seller.telefoon,
      openinghour: this.seller.openinghour
    }
  },
  computed: {
    smallScreen() {
      return screen.width <= 800;
    },
    mapwidth() {
      if (screen.width <= 800) {
        return 400;
      } else {
        return 600;
      }
    },
    mapheight() {
      if (screen.width <= 800) {
        return 300;
      } else {
        return 450;
      }
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
  },
  methods: {
    _initScroll() {
      // alert('ok')
      // console.log(`_initScrollseller ${this.data.monthmenu.title},`)
      // let smallScreen = screen.width <= 800;
      // console.log(`screen width ${screen.width}, smallScreen ${this.smallScreen}`)
      this.foodsScroll = new BScroll(this.$refs.ratingsWrapper, {
        click: true,
        probeType: 3,
        scrollbar: {
          fade: this.smallScreen,
          interactive: !this.smallScreen // new in 1.8.0
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
