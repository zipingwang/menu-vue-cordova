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
      url: this.data.options.signalrUrl,
      simpleHubProxy: null,
      connectionId: ''
    }
  },
  computed: {
  },
  created() {
    console.log('singalr created');
    this.$root.eventHub.$on('signalr.sendOrder', this.sendOrder)
    this.$root.eventHub.$on('signalr.registerUser', this.registerUser)
    this.$root.eventHub.$on('signalr.signin', this.signin)
    this.$root.eventHub.$on('signalr.downloadOrder', this.downloadOrder)
    this.$root.eventHub.$on('signalr.closeOrder', this.closeOrder)
    this.$root.eventHub.$on('signalr.deleteOrder', this.deleteOrder)
    this.$root.eventHub.$on('signalr.getTakeawayTimeSlots', this.getTakeawayTimeSlots)
    this.$root.eventHub.$on('signalr.sendMessageFromWebToServer', this.sendMessageFromWebToServer)
    this.$root.eventHub.$on('signalr.checkConnect', this.checkConnect)
    this.$root.eventHub.$on('signalr.publishMenu', this.publishMenu)
  },
  methods: {
    connectToSignalRServer() {
      let url = '' // 'http://localhost:44337/signalr';
      url = this.data.options.signalrUrl
      console.log('connectToSignalRServer')
      // console.log(url)
      // console.log(url)
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
      var tempSellerRid = this.data.options.shopRid
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
        tempSimpleHubProxy.client.onConnected = function (id, userName) {
          // this.writeToLog('onconneccted' + name + ':' + message);
          // alert('onconneccted' + name + ':' + message)
          // Handler['tempWriteLog']('onconnected' + name + ':' + message);
          // tempWriteLog('onconnected' + name + ':' + message);
          tempOnConnnected(id, userName)
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
        console.log('starting')
        // Connect to hub
        $.connection.hub.start().done(function() {
          // this.writeToLog('Connected.');
          // alert('Connected ok')
          // if (this) {
          //   alert('not null')
          // } else {
          //   alert('null')orderConfirmedFromServerToWeb
          // }
          // alert(tempSimpleHubProxy)
          console.log('started')
          tempSimpleHubProxy.server.connect('vue', tempSellerRid);
          // tempDownloadMenu();
        }).fail((err) => {
          console.log('fail to connect signalr')
          console.log(err)
        }
        );
      });

      // alert(tempSimpleHubProxy)
    },
    onConnected(id, userName) {
      this.connectionId = id
      // alert('onConnected:' + this.connectionId)
    },
    sendOrder(order) {
      try {
        console.log('send order in signr')
        this.simpleHubProxy.server.orderFromWebToServer(this.connectionId, this.getDataOptionsString(), order);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    registerUser(User) {
      try {
        this.simpleHubProxy.server.registerUserFromWebToServer(this.connectionId, this.getDataOptionsString(), User);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    signin(user) {
      try {
        this.simpleHubProxy.server.signinFromWebToServer(this.connectionId, this.getDataOptionsString(), user);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    downloadMenu() {
      // alert('downloadMenu')
      // console.log(this.data.options)
      // this.simpleHubProxy.server.downloadMenu(''); // this.data.options);
      try {
        this.simpleHubProxy.server.downloadMenu(this.getDataOptionsString());
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    downloadOrder() {
      // alert('downloadOrder')
      try {
        this.simpleHubProxy.server.downloadOrder(this.getDataOptionsString());
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    closeOrder(orderId) {
      // alert('downloadOrder')
      try {
        this.simpleHubProxy.server.closeOrder(this.getDataOptionsString(), orderId);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    deleteOrder(orderId) {
      // alert('downloadOrder')
      try {
        this.simpleHubProxy.server.deleteOrder(this.getDataOptionsString(), orderId);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    getTakeawayTimeSlots(order) {
      console.log('getTakeawayTimeSlots in signalr')
      try {
        this.simpleHubProxy.server.getTakeawayTimeSlots(this.getDataOptionsString(), order);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    sendMessageFromWebToServer(message) {
      try {
        this.simpleHubProxy.server.sendMessageFromWebToServer(this.getDataOptionsString(), message);
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    },
    onRegisterUserConfirmedFromServerToWeb(webClientConnectionId, userId, sessionId) {
      // alert('onRegisterUserConfirmedFromServerToWeb')
      if (this.connectionId === webClientConnectionId) {
        this.$root.eventHub.$emit('signalr.onRegisterUserConfirmedFromServerToWeb', userId, sessionId)
      }
    },
    onSigninConfirmedFromServerToWeb(webClientConnectionId, responsedata) {
      // alert('onSigninConfirmedFromServerToWeb')
      // console.log(typeof responsedata);
      // var s = '{"rid":"2020","isAdmin":"0"}'
      // console.log(typeof s);
      // var sobject = JSON.parse(s)
      // console.log(sobject)
      // console.log(responsedata)
      var responseobject = JSON.parse(responsedata)
      // console.log(responseobject)
      if (this.connectionId === webClientConnectionId) {
        console.log('onSigninConfirmedFromServerToWeb2 userid:');
        this.$root.eventHub.$emit('signalr.onSigninConfirmedFromServerToWeb', responseobject)
      }
    },
    onOrderConfirmedFromServerToWeb(webClientConnectionId, order, addremove) {
      console.log('onOrderConfirmedFromServerToWeb in signalr')
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
      // console.log(messageString)
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
            this.$root.eventHub.$emit('signalr.onOrderDownloaded', message.messageBody)
            break;
          case 'broadcastOrder':
            this.$root.eventHub.$emit('signalr.onBroadcastOrder', message.messageBody)
            break;
          case 'getTakeawayTimeSlots':
            console.log('call back getTakeawayTimeSlots in signalr')
            this.$root.eventHub.$emit('signalr.onGetTakeawayTimeSlots', message.messageBody)
            break;
          case 'saveBusinessInfo':
            this.$root.eventHub.$emit('signalr.onSaveBusinessInfo', message.messageBody)
            break;
          case 'downLoadBusinessInfo':
            console.log('call back downLoadBusinessInfo in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            break;
          case 'downLoadMenuGroups':
            console.log('call back downLoadMenuGroups in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadMenuGroups', message.messageBody)
            break;
          case 'saveMenuGroup':
            this.$root.eventHub.$emit('signalr.onSaveMenuGroup', message.messageBody)
            break;
          case 'deleteMenuGroup':
            this.$root.eventHub.$emit('signalr.onDeleteMenuGroup', message.messageBody)
            break;
          case 'addMenuGroup':
            this.$root.eventHub.$emit('signalr.onAddMenuGroup', message.messageBody)
            break;
          case 'downLoadRiceTables':
            console.log('call back downLoadRiceTables in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadRiceTables', message.messageBody)
            break;
          case 'saveRiceTable':
            this.$root.eventHub.$emit('signalr.onSaveRiceTable', message.messageBody)
            break;
          case 'deleteRiceTable':
            this.$root.eventHub.$emit('signalr.onDeleteRiceTable', message.messageBody)
            break;
          case 'addRiceTable':
            this.$root.eventHub.$emit('signalr.onAddRiceTable', message.messageBody)
            break;
          case 'downLoadSpecialDays':
            console.log('call back downLoadSpecialDays in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadSpecialDays', message.messageBody)
            break;
          case 'saveSpecialDay':
            this.$root.eventHub.$emit('signalr.onSaveSpecialDay', message.messageBody)
            break;
          case 'deleteSpecialDay':
            this.$root.eventHub.$emit('signalr.onDeleteSpecialDay', message.messageBody)
            break;
          case 'addSpecialDay':
            this.$root.eventHub.$emit('signalr.onAddSpecialDay', message.messageBody)
            break;
          case 'downLoadMenus':
            console.log('call back downLoadMenus in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadMenus', message.messageBody)
            break;
          case 'saveMenu':
            this.$root.eventHub.$emit('signalr.onSaveMenu', message.messageBody)
            break;
          case 'deleteMenu':
            this.$root.eventHub.$emit('signalr.onDeleteMenu', message.messageBody)
            break;
          case 'addMenu':
            this.$root.eventHub.$emit('signalr.onAddMenu', message.messageBody)
            break;
          case 'deleteMenuImage':
            this.$root.eventHub.$emit('signalr.onDeleteMenuImage', message.messageBody)
            break;
          case 'publishMenu':
            this.$root.eventHub.$emit('signalr.onPublishMenu', message.messageBody)
            break;
          case 'saveOpeninghour':
            this.$root.eventHub.$emit('signalr.onSaveOpeninghour', message.messageBody)
            break;
          case 'downLoadOpeninghour':
            console.log('call back downLoadOpeninghour in signalr')
            // this.$root.eventHub.$emit('signalr.onDownLoadBusinessInfo', message.messageBody)
            this.$root.eventHub.$emit('signalr.onDownLoadOpeninghour', message.messageBody)
            break;
          case 'webOrder':
            console.log('case webOrder')
            this.$root.eventHub.$emit('signalr.onWebOrderConfirmed', message.messageBody)
            break;
          case 'sessionExpired':
            console.log('sessionExpired')
            this.$root.eventHub.$emit('signalr.onSessionExpired', message.messageBody)
            break;
          case 'downLoadCustomers':
            console.log('call back downLoadCustomers in signalr')
            this.$root.eventHub.$emit('signalr.onDownLoadCustomers', message.messageBody)
            break;
          case 'saveCustomer':
            this.$root.eventHub.$emit('signalr.onSaveCustomer', message.messageBody)
            break;
          case 'deleteCustomer':
            this.$root.eventHub.$emit('signalr.onDeleteCustomer', message.messageBody)
            break;
          case 'checkOutValidationError':
            this.$root.eventHub.$emit('signalr.onCheckOutValidationError', message.messageBody)
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
    },
    checkConnect() {
      try {
        console.log('checkConnect')
        if (this.simpleHubProxy == null) {
          this.connectToSignalRServer();
        } else {
          this.simpleHubProxy.server.connect('vue', this.data.options.shopRid);
        }
      } catch (ex) {
        console.log(ex)
        this.simpleHubProxy = null
        this.connectToSignalRServer()
      }
    },
    publishMenu() {
      try {
        console.log('publishMenu')
        // console.log(this.getDataOptionsString())
        this.simpleHubProxy.server.publishMenu(this.getDataOptionsString())
      } catch (ex) {
        console.log(ex)
        this.connectToSignalRServer()
      }
    }
  }
}

</script>

<style lang="stylus" scoped>
</style>
