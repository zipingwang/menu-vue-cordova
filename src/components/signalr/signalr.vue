<template lang="html">
  <div></div>
</template>

<script>
import axios from 'axios'

export default {
  components: {
  },
  props: {
    seller: {},
    ml: {},
    data: {}
  },
  data() {
    return {
      url: this.seller.sellerurl,
      // userName: 'vue app',
      simpleHubProxy: null,
      connectionId: ''
    }
  },
  computed: {
  },
  created() {
    this.connectToSignalRServer()
    this.$root.eventHub.$on('signalr.sendOrder', this.sendOrder)
    this.$root.eventHub.$on('signalr.registerUser', this.registerUser)
    this.$root.eventHub.$on('signalr.signin', this.signin)
    this.$root.eventHub.$on('signalr.downloadOrder', this.downloadOrder)
    this.$root.eventHub.$on('signalr.closeOrder', this.closeOrder)
    this.$root.eventHub.$on('signalr.deleteOrder', this.deleteOrder)
    this.$root.eventHub.$on('signalr.getTakeawayTimeSlots', this.getTakeawayTimeSlots)
  },
  methods: {
    connect() {
      if (!this.simpleHubProxy) {
        // alert('simpleHubProxy is null')
        this.connectToSignalRServer();
      }
      this.sendOrder()
    },
    connectToSignalRServer() {
      // alert('click 2');
      let url = 'http://localhost:44337/signalr';
      // let url = '/signalr';
      // let url = 'http://www.freeobject.com/signalr';
      // let url = this.url + '/signalr';
      var Handler = {}
      // Handler.tempWriteLog = this.writeToLog
      var tempWriteLog = this.writeToLog
      var tempOnConnnected = this.onConnected
      var tempDownloadMenu = this.downloadMenu
      var temponOrderConfirmedFromServerToWeb = this.onOrderConfirmedFromServerToWeb
      var temponRegisterUserConfirmedFromServerToWeb = this.onRegisterUserConfirmedFromServerToWeb
      var temponSigninConfirmedFromServerToWeb = this.onSigninConfirmedFromServerToWeb
      var temponMenuDownloaded = this.onMenuDownloaded
      var temponMessageReceived = this.onMessageReceived

      var tempSimpleHubProxy = this.simpleHubProxy
      var tempSetSimpleHubProxy = this.setSimpleHubProxy
      // Handler.tempWriteLog = function (name) {
      //   console.log(name)
      // }
      $.getScript(url + '/hubs', function() {
        $.connection.hub.url = url;
        // alert(this.writeToLog)
        // Declare a proxy to reference the hub.
        // this.simpleHubProxy = $.connection.chatHub;
        tempSimpleHubProxy = $.connection.orderHub;
        // alert(tempSimpleHubProxy)
        tempSetSimpleHubProxy(tempSimpleHubProxy)
        // tempSimpleHubProxy = $.connection.chatHub;
        // tempSimpleHubProxy = this.simpleHubProxy;
        // alert(this.simpleHubProxy);
        // Reigster to the "AddMessage" callback method of the hub
        // This method is invoked by the hub
        tempSimpleHubProxy.client.addMessage = function (name, message) {
          this.writeToLog(name + ':' + message);
        };
        tempSimpleHubProxy.client.onConnected = function (id, userName, connectedusers) {
          // this.writeToLog('onconneccted' + name + ':' + message);
          // alert('onconneccted' + name + ':' + message)
          // Handler['tempWriteLog']('onconnected' + name + ':' + message);
          // tempWriteLog('onconnected' + name + ':' + message);
          tempOnConnnected(id, userName, connectedusers)
        };
        tempSimpleHubProxy.client.orderConfirmedFromServerToWeb = function (webClientConnectionId, order, addremove) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          temponOrderConfirmedFromServerToWeb(webClientConnectionId, order, addremove)
        };
        tempSimpleHubProxy.client.onRegisterUserConfirmedFromServerToWeb = function (webClientConnectionId, userId, sessionId) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          temponRegisterUserConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId)
        };
        tempSimpleHubProxy.client.onSigninConfirmedFromServerToWeb = function (webClientConnectionId, response) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          // alert('onSigninConfirmedFromServerToWeb userid:' + userId + 'sessionId:' + sessionId + 'connectionId:' + webClientConnectionId);
          temponSigninConfirmedFromServerToWeb(webClientConnectionId, response)
        };
        tempSimpleHubProxy.client.onMenuDownloaded = function (webClientConnectionId, data) {
          // alert('tempSimpleHubProxy.client.onMenuDownloaded')
          // alert('onSigninConfirmedFromServerToWeb userid:' + userId + 'sessionId:' + sessionId + 'connectionId:' + webClientConnectionId);
          temponMenuDownloaded(webClientConnectionId, data)
        };
        tempSimpleHubProxy.client.onReceiveMessage = function (webClientConnectionId, message) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // tempWriteLog('messagereceived' + name + ':' + message);
          temponMessageReceived(webClientConnectionId, message)
        };
        // Connect to hub
        $.connection.hub.start().done(function() {
          // this.writeToLog('Connected.');
          // alert('Connected ok')
          // if (this) {
          //   alert('not null')
          // } else {
          //   alert('null')
          // }
          // alert(tempSimpleHubProxy)

          tempSimpleHubProxy.server.connect('vue');
          // tempDownloadMenu();
        });
      });

      // alert(tempSimpleHubProxy)
    },
    onConnected(id, userName, allConnectedUsers) {
      this.connectionId = id
      // alert('onConnected:' + this.connectionId)
    },
    sendOrder(order) {
      this.simpleHubProxy.server.orderFromWebToServer(this.connectionId, this.getDataOptionsString(), order);
    },
    registerUser(User) {
      this.simpleHubProxy.server.registerUserFromWebToServer(this.connectionId, this.getDataOptionsString(), User);
    },
    signin(user) {
      this.simpleHubProxy.server.signinFromWebToServer(this.connectionId, this.getDataOptionsString(), user);
    },
    downloadMenu() {
      // alert('downloadMenu')
      // console.log(this.data.options)
      // this.simpleHubProxy.server.downloadMenu(''); // this.data.options);
      this.simpleHubProxy.server.downloadMenu(this.getDataOptionsString());
    },
    downloadOrder() {
      // alert('downloadOrder')
      this.simpleHubProxy.server.downloadOrder(this.getDataOptionsString());
    },
    closeOrder(orderId) {
      // alert('downloadOrder')
      this.simpleHubProxy.server.closeOrder(this.getDataOptionsString(), orderId);
    },
    deleteOrder(orderId) {
      // alert('downloadOrder')
      this.simpleHubProxy.server.deleteOrder(this.getDataOptionsString(), orderId);
    },
    getTakeawayTimeSlots(order) {
      console.log('getTakeawayTimeSlots in signalr')
      alert('getTakeawayTimeSlots in signalr')
      this.simpleHubProxy.server.getTakeawayTimeSlots(this.getDataOptionsString(), order);
    },
    onRegisterUserConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId) {
      // alert('onRegisterUserConfirmedFromServerToWeb')
      if (this.connectionId === webClientConnectionId) {
        this.$root.eventHub.$emit('signalr.onRegisterUserConfirmedFromServerToWeb', userId, sessionId)
      }
    },
    onSigninConfirmedFromServerToWeb(webClientConnectionId, responsedata) {
      // alert('onSigninConfirmedFromServerToWeb')
      console.log(typeof responsedata);
      var s = '{"rid":"2020","isAdmin":"0"}'
      console.log(typeof s);
      var sobject = JSON.parse(s)
      console.log(sobject)
      console.log(responsedata)
      var responseobject = JSON.parse(responsedata)
      console.log(responseobject)
      if (this.connectionId === webClientConnectionId) {
        console.log('onSigninConfirmedFromServerToWeb2 userid:');
        this.$root.eventHub.$emit('signalr.onSigninConfirmedFromServerToWeb', responseobject)
      }
    },
    onOrderConfirmedFromServerToWeb(webClientConnectionId, order, addremove) {
      if (this.connectionId === webClientConnectionId) {
        this.$root.eventHub.$emit('signalr.onOrderConfirmedFromServerToWeb', order, addremove)
        // alert('onOrderConfirmedFromServerToWeb:' + webClientConnectionId)
        // this.$Modal.success({
        //   title: 'Success',
        //   content: '<p>Order is geplaast signalr</p>'
        // });
      }
    },
    onMenuDownloaded(webClientConnectionId, data) {
      // alert('onRegisterUserConfirmedFromServerToWeb')
      this.$root.eventHub.$emit('signalr.downloaded', data)
    },
    onMessageReceived(webClientConnectionId, messageString) {
      console.log(messageString)
      var message = JSON.parse(messageString)
      // console.log(message)
      // alert(message.status)
      if (message.status === 'ok') {
        // if (message.messageType === 'downloadorder') {
        //   // alert('before emit signalr.orderDownloaded ')
        //   // console.log(message.messageBody)
        //   this.$root.eventHub.$emit('signalr.orderDownloaded', message.messageBody)
        //   // alert('after emit signalr.orderDownloaded ')
        // }
        switch (message.messageType) {
          case 'downloadOrder':
            this.$root.eventHub.$emit('signalr.orderDownloaded', message.messageBody)
            break;
          case 'broadcastOrder':
            this.$root.eventHub.$emit('signalr.broadcastOrder', message.messageBody)
            break;
          case 'getTakeawayTimeSlots':
            console.log('call back getTakeawayTimeSlots in signalr')
            this.$root.eventHub.$emit('signalr.onGetTakeawayTimeSlots', message.messageBody)
            break;
          default:
            // code block
        }
      }
    },
    getDataOptionsString() {
      return JSON.stringify(this.data.options);
    },
    writeToLog(msg) {
      console.log(msg)
    },
    setSimpleHubProxy(proxy) {
      this.simpleHubProxy = proxy;
    }
  }
}

</script>

<style lang="stylus" scoped>
</style>
