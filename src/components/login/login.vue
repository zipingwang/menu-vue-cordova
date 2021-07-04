<template lang="html">
  <div>
  <transition name="fade">
    <div v-show="show" class="detail" @click="hidelogin()">
    <!-- <div class="detail" @click="showToggle()"> -->
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="show">
      <div class="billcontainer">
          <div class="columnpadding"></div>
          <div class="billcontent">
            <backButton @click="hidelogin()"></backButton>
            <card class="customer">
                <p slot="title">{{ml.login}}</p>
                <i-form ref="formInline" :model="formInline" :rules="ruleInline" inline>
                    <form-item prop="user">
                        <i-input type="text" v-model="formInline.user" :placeholder="ml.loginnameplaceholder">
                            <icon type="ios-person-outline" slot="prepend"></icon>
                        </i-input>
                    </form-item>
                    <form-item prop="password">
                        <!-- <i-input type="password" v-model="formInline.password" :placeholder="loginpasswordlaceholder" > -->
                        <i-input type="password" v-model="formInline.password" >
                            <icon type="ios-lock-outline" slot="prepend"></icon>
                        </i-input>
                    </form-item>
                </i-form>
                <sendButton ref="myLoginButton" :text="ml.login" :ml="ml" :sendingText="ml.sending" :failedText="ml.logincommunicationfailed" @click="handleSubmit('formInline')"></sendButton>
                <span class="forgotPassword"><a @click="forgotPassword">{{ml.forgotpassword}}</a></span>
            </card>
            <div class="notcustomeryet">
            <card>
                <p slot="title">{{ml.noggeenaccount}}</p>
                <Button type="primary" class="register" @click="register()">{{ml.register}}</Button>
                <!-- <Button type="primary" class="loginasgust" @click="loginasguest()">{{ml.loginasguest}}</Button> -->
                <!-- <Button type="primary" class="close" @click="hidelogin()">{{ml.cancel}}</Button>             -->
            </card>

            </div>
          </div>
          <div class="columnpadding"></div>
          <!-- <div class="button">

          </div> -->
      </div>

    </div>
  </transition>
  <register ref="myregister" :seller="seller" :data="data" :ml="ml" @close="onRegisterClose"></register>
  <forgotPassword ref="myForgotPassword" :seller="seller" :data="data" :ml="ml" @close="onForgotPasswordClose"></forgotPassword>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'
import axios from 'axios'
import register from 'components/register/register'
import sendButton from 'components/common/sendButton/sendButton'
import forgotPassword from 'components/forgotPassword/forgotPassword'
import backButton from 'components/common/backButton/backButton'

export default {
  components: {
    register,
    sendButton,
    forgotPassword,
    backButton
  },
  props: {
    seller: {},
    data: {},
    ml: {}
  },
  data() {
    return {
      show: false,
      userName: 'vue app',
      simpleHubProxy: null,
      connectionId: '',
      formInline: {
        user: '',
        password: ''
      },
      ruleInline: {
        user: [
          { required: true, message: this.ml.requiredfield, trigger: 'blur' }
        ],
        password: [
          { required: true, message: this.ml.requiredfield, trigger: 'blur' }
          // { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    signinString() {
      let requestString = ''
      requestString = JSON.stringify(this.formInline)
      return requestString
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    this.$root.eventHub.$on('signalr.onSigninConfirmedFromServerToWeb', this.onSigninConfirmedFromServerToWeb)
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
    register() {
      this.show = false
      this.$refs.myregister.showregister()
    },
    loginasguest() {
      // alert('loginasguest')
      // this.$emit('loginevent', 'loginasguest')
      this.show = false
      data.options.cusId = 'guest'
      data.options.isAdmin = '0'
      this.$root.eventHub.$emit('login.loggedin', {'rid': 'guest', 'isAdmin': '0'})
    },
    showlogin() {
      this.show = true;
      this.$root.eventHub.$emit('signalr.connect')
      this.$nextTick(() => {
        this.foodsScroll.refresh(); // 初始化scroll
      })
    },
    hidelogin() {
      this.show = false;
      this.data.ruleInline = {
        user: [
          { required: true, message: this.ml.requiredfield, trigger: 'blur' }
        ],
        password: [
          { required: true, message: this.ml.requiredfield, trigger: 'blur' }
          // { type: 'string', min: 6, message: 'The password length cannot be less than 6 bits', trigger: 'blur' }
        ]
      }
    },
    onRegisterClose() {
      this.show = true
    },
    onForgotPasswordClose() {
      this.show = true
    },
    handleSubmit(name) {
      this.$refs[name].validate((valid) => {
        if (valid) {
          console.log('this.signinString');
          this.$refs.myLoginButton.start()
          // Console.log(this.testmethod); /* fataal error. console is lowercase, not Console */
          // this.busyWithSending = true
          // this.startTime = new Date()
          // this.mySendingTimer = setInterval(this.checkSending, 1000)
          this.$root.eventHub.$emit('signalr.signin', this.signinString);
        } else {
          this.busyWithSending = false
          this.$Message.error(this.ml.formvalidationerror);
        }
      })
    },
    forgotPassword() {
      this.show = false
      this.$refs.myForgotPassword.showForgotPassword()
    },
    // checkSending() {
    //   console.log('checkSending')
    //   if (this.startTime.setSeconds(this.startTime.getSeconds() + 10) < new Date()) {
    //     this.busyWithSending = false
    //     clearInterval(this.mySendingTimer)
    //     // this.showWaiting = false
    //     this.$Modal.success({
    //       title: this.ml.failed,
    //       content: this.ml.failed
    //     });
    //   } else {
    //     this.startTime.setSeconds(this.startTime.getSeconds() - 10) /* set to its original time */
    //   }
    // },
    onSigninConfirmedFromServerToWeb(user) {
      // alert(userId)
      // this.busyWithSending = false
      // clearInterval(this.mySendingTimer)
      if (this.$refs.myLoginButton === undefined) {
        console.log('this.$refs.myLoginButton === undefined')
      } else {
        this.$refs.myLoginButton.stop()
      }
      /* -1: user not exist, -2 user exists, but connection not same as connet */
      if (user.rid !== '-1' && user.rid !== '-2') {
        this.show = false
        // this.data.options.cusId = user.rid /* don't change parent data */
        // this.data.options.isAdmin = user.isAdmin
        // alert('user logged in')
        this.$root.eventHub.$emit('login.loggedin', user)
        // this.$root.eventHub.$emit('signalr.downloadOrder')
        // this.$Modal.success({
        //   title: this.ml.success,
        //   content: '<p>onSigninConfirmedFromServerToWeb is geplaast</p>'
        // });
      } else {
        this.$Modal.success({
          title: this.ml.failed,
          content: this.ml.loginfailed,
          okText: this.ml.ok
        });
      }
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
  margin: 50px auto
}
.billcontent {
  flex: 80%
}
.loginasguest {
  margin: 10px 20px
}
.rowheader {
  display: flex;
  flex-wrap: nowrap;
  margin-bottom : 20px;
  padding-bottom : 10px;
  padding-top: 20px;
  border-bottom : 1px dashed
}
.footline {
  display: flex;
  flex-wrap: nowrap;
  padding-top: 20px;
  border-top : 1px dashed
}

/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: nowrap;
  margin-bottom : 10px;
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
.ordertext {
  margin-top: 10px;
  text-align: center;
}
.buttonarea
{
  text-align: center;
  margin-top: 20px;
}
.close {
  margin: 10px 20px
}
.forgotPassword {
  margin-left: 30px
}
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
  // bottom 48px
  bottom 0
  width 100%
  z-index 130
  background white
  color rgb(77,85,93)
  transition all 0.4s ease
  &.move-enter-avtive,&.move-leave{
    transform translate3d(0,0,0)
  }
  &.move-enter,&.move-leave-active{
    transform translate3d(100%,0,0)
  }
  .billcontainer
      .billcontent
        .notcustomeryet
          margin 20px auto
        .register
          margin 10px 20px

@media screen and (min-width: 800px)
  .detailWrapper
    position fixed
    top 0
    left 20%
    margin-right -20%
    // bottom 48px
    bottom 0
    width 100%
    max-width: 800px
    background white
    color rgb(77,85,93)
    transition all 0.4s ease
    &.move-enter-avtive,&.move-leave{
      transform translate3d(0,0,0)
    }
    &.move-enter,&.move-leave-active{
      transform translate3d(10%,0,0)
    }



</style>
