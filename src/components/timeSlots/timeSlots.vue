<template lang="html">
  <div>
  <transition name="fade">
    <div v-show="show" class="detail" @click="hideTimeSlots()">
    <!-- <div class="detail" @click="showToggle()"> -->
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="show">
      <div class="billcontainer">
          <div class="columnpadding"></div>
          <div class="billcontent">
            <ul>
              <li class="row" v-for="item in takeawayTimeSlots" @click="selectTimeSlot(item)">
                <span class="timeSlot">{{item}}</span>
              </li>
            </ul>
          </div>
          <div class="columnpadding"></div>
      </div>
    </div>
  </transition>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'
import axios from 'axios'
import backdrop from 'components/backdrop/backdrop'

export default {
  components: {
    backdrop
  },
  props: {
    // seller: {},
    // timeSlots: [],
    // ml: {},
    // data: {},
    takeawayTimeSlots: {
      type: Array,
      default: []
    }
  },
  data() {
    return {
      show: false,
      showWaiting: false,
      takeawayTimeSlot: '1200'
      // takeawayTimeSlots: []
    }
  },
  mounted() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
  },
  methods: {
    _initScroll() {
      let smallScreen = screen.width <= 800;
      console.log(`screen width ${screen.width}, smallScreen ${smallScreen}`)
      this.foodsScroll = new BScroll(this.$refs.detailWrapper, {
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
    },
    hideTimeSlots() {
      this.show = false
    },
    selectTimeSlot(timeSlot) {
      this.$emit('timeSlotSelected', timeSlot)
      this.hideTimeSlots()
    },
    showTimeSlots() {
      this.show = true
      setTimeout(
        () => { this.foodsScroll.refresh() }, 200) /* must wait some time, because it'a parant has translation animation */
      // this.foodsScroll.refresh()
      // this.$nextTick(() => {
      //   this._initScroll(); // 初始化scroll
      // })
    }
  }
}

</script>

<style lang="stylus" scoped>

* {
  box-sizing: border-box;
}
.billcontainer {
  display: flex;
  flex-wrap: nowrap;
}
.billcontent {
  flex: 80%
  padding: 10px 10px
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: nowrap;
  margin-top : 10px;
  line-height: 30px;
}

/* Create four equal columns */
.columncount {
  flex: 10%;
  //text-align right;
  //padding-right 20px;
}

.columnname {
  flex: 70%;
}
.columnunitprice {
  flex: 10%;
}
.columnprice {
  flex: 10%;
}
.columnpadding {
  flex : 10%
}
.totalline
{
  margin-top: 40px
  text-align: center;
  font-size: 24px;
}
.backdrop {
  z-index 160
}
.detail
  position fixed
  left 0
  top 0
  height 100%
  width 100%
  z-index 150
  background rgba(7,17,27,0.8)
  // backdrop-filter blur(2px)
  transition all 0.4s ease
@media screen and (min-width: 800px)
  .detail
      position fixed
      left 0
      top 0
      height 100%
      width 100%
      z-index 150
      background rgba(7,17,27,0.8)
      // backdrop-filter blur(2px)
      transition all 0.4s ease
.detailWrapper
  // position fixed
  // left 0
  // top 0
  // bottom 48px
  // width 50%
  // z-index 150
  // background white
  // color rgb(77,85,93)
  width:200px;
  height:400px;
  position: fixed;
  background-color: #00FF00;
  top: 50%;
  left: 50%;
  margin-top: -200px;
  margin-left: -100px;
  z-index 150
  background white
  overflow hidden
  transition all 0.4s ease
  &.move-enter-avtive,&.move-leave{
    transform translate3d(0,0,0)
  }
  &.move-enter,&.move-leave-active{
    transform translate3d(100%,0,0)
  }
@media screen and (min-width: 800px)
  .detailWrapper
    width:200px;
    height:400px;
    position: fixed;
    background-color: #00FF00;
    top: 50%;
    left: 50%;
    margin-top: -200px;
    margin-left: -100px;
    z-index 150
    background white
    overflow hidden
    transition all 0.4s ease
    &.move-enter-avtive,&.move-leave{
      transform translate3d(0,0,0)
    }
    &.move-enter,&.move-leave-active{
      transform translate3d(10%,0,0)
    }

</style>
