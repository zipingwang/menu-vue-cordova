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
    ml: {}
  },
  data() {
    return {
      show: false,
      url: this.seller.sellerurl,
      userName: 'vue app',
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
  },
  methods: {
    connect() {
      if (!this.simpleHubProxy) {
        alert('simpleHubProxy is null')
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
      var temponOrderConfirmedFromServerToWeb = this.onOrderConfirmedFromServerToWeb
      var temponRegisterUserConfirmedFromServerToWeb = this.onRegisterUserConfirmedFromServerToWeb
      var temponSigninConfirmedFromServerToWeb = this.onSigninConfirmedFromServerToWeb

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
        tempSimpleHubProxy.client.orderConfirmedFromServerToWeb = function (webClientConnectionId, orderId) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          temponOrderConfirmedFromServerToWeb(webClientConnectionId, orderId)
        };
        tempSimpleHubProxy.client.onRegisterUserConfirmedFromServerToWeb = function (webClientConnectionId, userId, sessionId) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          temponRegisterUserConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId)
        };
        tempSimpleHubProxy.client.onSigninConfirmedFromServerToWeb = function (webClientConnectionId, userId, sessionId) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          // alert('tempSimpleHubProxy.client.orderConfirmedFromServerToWeb')
          // alert('onSigninConfirmedFromServerToWeb userid:' + userId + 'sessionId:' + sessionId + 'connectionId:' + webClientConnectionId);
          temponSigninConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId)
        };
        tempSimpleHubProxy.client.messageReceived = function (name, message, time, userimg) {
          // Handler['tempWriteLog']('messagereceived' + name + ':' + message);
          tempWriteLog('messagereceived' + name + ':' + message);
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
        });
      });

      // alert(tempSimpleHubProxy)
    },
    onConnected(id, userName, allConnectedUsers) {
      this.connectionId = id
      // alert('onConnected:' + this.connectionId)
    },
    sendOrder(order) {
      this.simpleHubProxy.server.orderFromWebToServer(this.connectionId, 'vue', order);
    },
    registerUser(User) {
      this.simpleHubProxy.server.registerUserFromWebToServer(this.connectionId, 'vue', User);
    },
    signin(user) {
      this.simpleHubProxy.server.signinFromWebToServer(this.connectionId, 'vue', user);
    },
    onRegisterUserConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId) {
      // alert('onRegisterUserConfirmedFromServerToWeb')
      if (this.connectionId === webClientConnectionId) {
        this.$root.eventHub.$emit('signalr.onRegisterUserConfirmedFromServerToWeb', userId, sessionId)
      }
    },
    onSigninConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId) {
      // alert('onSigninConfirmedFromServerToWeb')
      if (this.connectionId === webClientConnectionId) {
        console.log('onSigninConfirmedFromServerToWeb2 userid:' + userId + 'sessionId:' + sessionId + 'connectionId:' + webClientConnectionId);
        this.$root.eventHub.$emit('signalr.onSigninConfirmedFromServerToWeb', userId, sessionId)
      }
    },
    onOrderConfirmedFromServerToWeb(webClientConnectionId, orderId) {
      if (this.connectionId === webClientConnectionId) {
        this.$root.eventHub.$emit('signalr.onOrderConfirmedFromServerToWeb', orderId)
        // alert('onOrderConfirmedFromServerToWeb:' + webClientConnectionId)
        // this.$Modal.success({
        //   title: 'Success',
        //   content: '<p>Order is geplaast signalr</p>'
        // });
      }
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
