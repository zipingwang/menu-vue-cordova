<template lang="html">
  <div>
  <transition name="fade">
    <div v-show="show" class="detail" @click="hideregister()">
    <!-- <div class="detail" @click="showToggle()"> -->
    </div>
  </transition>
  <transition name="move">
    <div class="detailWrapper" ref="detailWrapper" v-show="show">
      <div class="billcontainer">
          <div class="columnpadding"></div>
          <div class="billcontent">
            <card class="customer">
                <p slot="title">{{ml.forgotpassword}}forgotpassword</p>
                <p>{{ml.forgotpassword}}receiveemailtochangepassword</p>
                <br/>

                 <i-form ref="formItem1" :model="formItem" :rules="ruleValidate" :label-width="100">
                 <!-- <i-form ref="formItem1" :model="formItem" :label-width="80"> -->
                    <FormItem :label="ml.email" prop="email">
                        <i-input v-model="formItem.email" :placeholder="ml.email"></i-input>
                    </FormItem>
                    <form-item>
                        <sendButton ref="mySendButton" :ml="ml" :text="ml.resetpassword" :sendingText="ml.sending" :failedText="ml.communicationfailed"
                          @click="handleSubmit('formItem1')" :timeout="15"></sendButton>
                        <!-- <i-button type="primary" @click="handleSubmit('formItem2')">Submit</i-button> -->
                        <!-- <i-button @click="handleReset('formItem')" style="margin-left: 8px">{{ml.reset}}</i-button> -->
                        <i-button type="primary" @click="cancel" style="margin-left: 8px">{{ml.cancel}}</i-button>
                    </form-item>
                </i-form>
            </card>
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
import sendButton from 'components/common/sendButton/sendButton'


export default {
  components: {
    sendButton
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
      formItem: {
        email: ''
      },
      ruleValidate: {
        telephone: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 8, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
        email: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'email', message: this.invalidEailFormatString, trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    registerString() {
      let requestString = ''
      this.formItem['requesturl'] = this.data.options.requestUrl // dynamic insert, not put in url, in post body url auto encoded
      this.formItem['baseurl'] = this.data.options.baseUrl
      requestString = JSON.stringify(this.formItem)
      return requestString
    },
    invalidEailFormatString() {
      return '1234' // this.ml.incorrectemailformat
    }
  },
  created() {
    this.$nextTick(() => {
      this._initScroll(); // 初始化scroll
    })
    this.$root.eventHub.$on('signalr.onRegisterUserConfirmedFromServerToWeb', this.onRegisterUserConfirmedFromServerToWeb)
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
    showForgotPassword() {
      this.show = true;
      /* message is not changed if language changed. must assign again so that language change has effect  */
      this.ruleValidate = {
        telephone: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 8, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
        email: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'email', message: this.ml.incorrectemailformat, trigger: 'blur' }
        ]
      }
      this.$nextTick(() => {
        this.foodsScroll.refresh(); // 初始化scroll
      })
    },
    hideregister() {
      this.show = false;
    },
    handleSubmit(name) {
      var isvalid = false
      this.$refs.formItem1.validate((valid) => {
        isvalid = valid
      })
      if (isvalid) {
        this.$refs.mySendButton.start()
        // this.$root.eventHub.$emit('signalr.registerUser', this.registerString);
        this.requestChangePassword()
      } else {
        this.$Message.error(this.ml.formvalidationerror);
      }
    },
    cancel() {
      this.show = false
      this.$emit('close')
    },
    requestChangePassword() {
      console.log('request change password')
      axios.post(this.data.options.requestUrl + 'method=forgotpassword' + '&lnindex=' + this.data.currentlnindex + '&siterid=' + this.data.options.shopRid, this.registerString)
        .then((res) => {
          console.log('stop send')
          console.log(this)
          this.$refs.mySendButton.stop();
          console.log(res);
          if (res.data === 'Ok') {
            this.$Modal.success({
              title: this.ml.success,
              content: this.ml.resetpasswordemailsent,
              okText: this.ml.ok
            });
          } else {
            this.$Modal.warning({
              title: 'ml.failed',
              content: 'ml.resetpasswordemailsentfailed',
              okText: this.ml.ok
            });
          }
        })
        .catch((error) => {
          console.log(error);
          this.$Modal.warning({
            title: 'ml.failed',
            content: 'ml.resetpasswordemailsentfailed',
            okText: this.ml.ok
          });
        });
    },
    onRegisterUserConfirmedFromServerToWeb(userId, sessionId) {
      this.$refs.mySendButton.stop()
      if (userId === '-1') {
        this.$Modal.warning({
          title: 'ml.failed',
          content: 'ml.userregisterationfailed',
          okText: this.ml.ok
        });
      } else if (userId === '-2') { /* user already exsits */
        this.$Modal.warning({
          content: this.ml.useralreadyexists,
          okText: this.ml.ok
        });
      } else {
        this.$Modal.success({
          title: this.ml.success,
          content: this.ml.userregisterationsuccess,
          okText: this.ml.ok
        });
        this.show = false
        this.$emit('close')
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
  text-align: center;
  padding: 5px 10px;
  font-size: 12px;
  font-weight: 700;
  background: #00b43c;
  color: white;
  line-height: 48px;
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
