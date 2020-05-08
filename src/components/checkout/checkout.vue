<template lang="html">
  <div>
  <transition name="fade">
    <div v-show="show" class="detail" @click="hidecheckout()">
    <!-- <div class="detail" @click="showToggle()"> -->
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="show">
      <div class="list-content" ref="foodlist">
          <ul>
            <li class="food" v-for="food in selectFoods">
              <span class="name">{{food.name}}</span>
              <span class="price">{{food.count}}</span>
              <span class="subtotal">{{food.price * food.count}} </span>
            </li>
          </ul>
      </div>
      <div>
      <button type="button" @click="connect()">Ok</button>
      </div>
      <div>
      <button type="button" @click="hidecheckout()">Cancel</button>
      </div>
      <div class="detail-close">
        <i class="icon-close" @click="hidecheckout()"></i>
      </div>
    </div>
  </transition>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'

// Reference to simpleHub proxy
// var simpleHubProxy;
// var url = 'https://localhost:44337/signalr';
// var userName = 'vue app';

// function connect() {
//   alert('click');
//     // Load auto generated hub script dynamically and perform connection operation when loading completed
//     // SignalR server location is specified by 'Url' input element, hub script must be loaded from the same location
//     // For production, remove this call and uncomment the script block in the header part
//   $.getScript(url + '/hubs', function() {
//     $.connection.hub.url = url;
//     // Declare a proxy to reference the hub.
//     simpleHubProxy = $.connection.simpleHub;

//     // Reigster to the 'AddMessage' callback method of the hub
//     // This method is invoked by the hub
//     simpleHubProxy.client.orderReceivedInBackOffice = function (userName, order, time) {
//       alert('order confirmed');
//       // writeToLog(name + ':' + message);
//     };

//     // Connect to hub
//     $.connection.hub.start().done(function () {
//       // writeToLog('Connected.');
//       simpleHubProxy.server.Connect(userName);
//       simpleHubProxy.server.OrderFromWeb(userName, 'my order', '');
//     });
//   });
// }

// // Disconnect from the server
// function disconnect() {
//   if (simpleHubProxy != null) {
//     $.connection.hub.stop().done(function() {
//       simpleHubProxy = null;
//       // writeToLog("Disconnected.");
//     });
//   }
// }

// // Send a message to server
// function sendMessage() {
//   if (simpleHubProxy != null) {
//     simpleHubProxy.server.send($('#txtMessage').val());
//   }
// }

export default {
  components: {
  },
  props: {
    selectFoods: {
      type: Array,
      default: []
    },
    totalPrice: 0
  },
  data() {
    return {
      show: false,
      trans: ml.trans,
      simpleHubProxy: {},
      url: 'http://localhost/signalr',
      userName: 'vue app'
    }
  },
  computed: {
  },
  methods: {
    showcheckout() {
      this.show = true;
    },
    hidecheckout() {
      this.show = false;
    },
    connect() {
      alert('click');
        // Load auto generated hub script dynamically and perform connection operation when loading completed
        // SignalR server location is specified by 'Url' input element, hub script must be loaded from the same location
        // For production, remove this call and uncomment the script block in the header part
      $.getScript(this.url + '/hubs', function() {
        $.connection.hub.url = this.url;
        // Declare a proxy to reference the hub.
        this.simpleHubProxy = $.connection.orderHub;

        // Reigster to the 'AddMessage' callback method of the hub
        // This method is invoked by the hub
        // this.simpleHubProxy.client.orderReceivedInBackOffice = function (userName, order, time) {
        //   alert('order confirmed');
        //   // writeToLog(name + ':' + message);
        // };

        alert('before start connect to server');
        // Connect to hub
        // $.connection.hub.start().done(function () {
        //   alert('success');
        //   this.afterconnecttoserver()
        // }
        $.connection.hub.start().done(function () {
          alert('connected');
        });
        alert('after start connect to server');
      });
    },
    afterconnecttoserver() {
      alert('success server');
      this.simpleHubProxy.server.connect(this.userName);
      this.simpleHubProxy.server.orderFromWeb(this.userName, 'my order', '');
      alert('order send to server');
    },
    // Disconnect from the server
    disconnect() {
      if (this.simpleHubProxy != null) {
        $.connection.hub.stop().done(function() {
          this.simpleHubProxy = null;
          // writeToLog("Disconnected.");
        });
      }
    }
  }
}

</script>

<style lang="stylus" scoped>
.detail
  position fixed
  left 0
  top 0
  height 100%
  width 100%
  z-index 130
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
      z-index 130
      background rgba(7,17,27,0.8)
      // backdrop-filter blur(2px)
      transition all 0.4s ease
.detailWrapper
  position fixed
  left 0
  top 0
  bottom 48px
  width 100%
  z-index 130
  background white
  transition all 0.4s ease
  &.move-enter-avtive,&.move-leave{
    transform translate3d(0,0,0)
  }
  &.move-enter,&.move-leave-active{
    transform translate3d(100%,0,0)
  }
@media screen and (min-width: 800px)
  .detailWrapper
    position fixed
    top 0
    left 20%
    margin-right -20%
    bottom 48px
    width 100%
    max-width: 800px
    background white
    transition all 0.4s ease
    &.move-enter-avtive,&.move-leave{
      transform translate3d(0,0,0)
    }
    &.move-enter,&.move-leave-active{
      transform translate3d(10%,0,0)
    }
 .detail-close
    position relative
    width 32px
    height 32px
    margin -64px auto 0 auto
    clear both
    font-size 32px
    color rgba(0,0,0,0.5)


</style>
