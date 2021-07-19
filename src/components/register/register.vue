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
            <backButton @click="cancel()"></backButton>
            <card class="customer">
                <p slot="title">{{ml.register}}</p>
                 <i-form ref="formItem1" :model="formItem" :rules="ruleValidate" :label-width="120">
                 <!-- <i-form ref="formItem1" :model="formItem" :label-width="80"> -->
                      <FormItem :label="ml.createuserfirstname" prop="firstname">
                        <i-input v-model="formItem.firstname"></i-input>
                    </FormItem>
                    <FormItem :label="ml.createuserlastname" prop="lastname">
                        <i-input v-model="formItem.lastname"></i-input>
                    </FormItem>
                     <FormItem :label="ml.createusertelephone" prop="telephone">
                        <i-input v-model="formItem.telephone" ></i-input>
                    </FormItem>
                     <FormItem :label="ml.createuseremail" prop="email">
                        <i-input v-model="formItem.email"></i-input>
                    </FormItem>
    </i-form>
                 <i-form ref="formItem2" :model="formItem" :rules="ruleCustom" :label-width="120">
                   <!-- <i-form ref="formItem2" :model="formItem" :label-width="100"> -->
                      <!-- <FormItem :label="ml.createuserfirstname">
                        <i-input v-model="formItem.firstname" :placeholder="ml.createuserfirstname"></i-input>
                    </FormItem>
                    <FormItem :label="ml.createuserlastname">
                        <i-input v-model="formItem.lastname" :placeholder="ml.createuserlastname"></i-input>
                    </FormItem>
                     <FormItem :label="ml.createusertelephone" prop="telephone">
                        <i-input v-model="formItem.telephone" :placeholder="ml.createusertelephone"></i-input>
                    </FormItem>
                     <FormItem :label="ml.createuseremail" prop="email">
                        <i-input v-model="formItem.email"></i-input>
                    </FormItem> -->
        <form-item :label="ml.password" prop="password">
            <i-input type="password" v-model="formItem.password"></i-input>
        </form-item>
        <form-item :label="ml.confirmpassword" prop="passwordconfirm">
            <i-input type="password" v-model="formItem.passwordconfirm"></i-input>
        </form-item>
        <!-- <Checkbox v-model="formItem.agreeWithPrivacyPolicy"><span @click="openPrivacyPolicy" v-html="ml.privacypolicy">{{ml.privacypolicy}}{{useTermAndPrivacyPolicy}}</span></Checkbox> -->
        <Checkbox v-model="formItem.agreeWithPrivacyPolicy">

        </Checkbox>
        <useTermAndPrivacy :ml="ml" @openUseTerm="openUseTerm()" @openPrivacyPolicy="openPrivacyPolicy()"></useTermAndPrivacy>
        <!-- <span v-html="useTermAndPrivacyPolicy">{{ml.privacypolicy}}{{useTermAndPrivacyPolicy}}</span> -->
        <form-item>
            <sendButton ref="mySendButton" :text="ml.register" :disabled="!formItem.agreeWithPrivacyPolicy" :sendingText="ml.sending" :failedText="ml.userregistrationcommunicationfailed" @click="handleSubmit('formItem2')"></sendButton>
            <!-- <i-button type="primary" @click="handleSubmit('formItem2')">Submit</i-button> -->
            <!-- <i-button @click="handleReset('formItem')" style="margin-left: 8px">{{ml.reset}}</i-button> -->
            <!-- <i-button type="primary" @click="cancel" style="margin-left: 8px">{{ml.cancel}}</i-button> -->
        </form-item>
    </i-form>
            </card>
          </div>
          <div class="columnpadding"></div>
          <!-- <div class="button">

          </div> -->
      </div>

    </div>
  </transition>
   <fileViewer ref="myPrivacyPolicy" :seller="seller" :data="data" :ml="ml"></fileViewer>
  </div>
</template>

<script>
import '../../filter/time.js'
import BScroll from 'better-scroll'
import axios from 'axios'
import sendButton from 'components/common/sendButton/sendButton'
import fileViewer from 'components/fileViewer/fileViewer'
import backButton from 'components/common/backButton/backButton'
import useTermAndPrivacy from 'components/register/useTermAndPrivacy'


export default {
  components: {
    sendButton,
    fileViewer,
    backButton,
    useTermAndPrivacy
  },
  props: {
    seller: {},
    data: {},
    ml: {}
  },
  data() {
    const validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error(this.ml.requiredfield));
      } else if (value.length < 6) {
        callback(new Error(this.ml.minimumlengthrequired));
      } else {
        if (this.formItem.passwordconfirm !== '') {
          // 对第二个密码框单独验证
          this.$refs.formItem2.validateField('passwordconfirm');
        }
        callback();
      }
    };
    const validatePassCheck = (rule, value, callback) => {
      if (value === '') {
        callback(new Error(this.ml.requiredfield));
      } else if (value !== this.formItem.password) {
        callback(new Error(this.ml.passwordmismatch));
      } else {
        callback();
      }
    };
    return {
      show: false,
      userName: 'vue app',
      simpleHubProxy: null,
      connectionId: '',
      formItem: {
        title: '',
        firstname: '',
        lastname: '',
        birthday: '',
        birthdayday: '',
        birthdaymonth: '',
        birthdayyear: '',
        address: '',
        postcode: '',
        place: '',
        telephone: '',
        mobile: '',
        email: '',
        password: '',
        passwordconfirm: '',
        agreeWithPrivacyPolicy: false
      },
      ruleCustom: {
        password: [
          { required: true, validator: validatePass, trigger: 'blur' }
        ],
        passwordconfirm: [
          { required: true, validator: validatePassCheck, trigger: 'blur' }
        ]
        // email: [
        //   { validator: validateEmail, trigger: 'blur' }
        // ],
        // telephone: [
        //   { validator: validateTelephone, trigger: 'blur' }
        // ]
      },
      ruleValidate: {
        firstname: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 2, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
        lastname: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 2, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
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
      this.formItem['lnindex'] = this.data.currentlnindex
      this.formItem['siterid'] = this.data.options.shopRid
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
    showregister() {
      this.show = true;
      /* message is not changed if language changed. must assign again so that language change has effect  */
      this.ruleValidate = {
        firstname: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 2, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
        lastname: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' },
            { type: 'string', min: 2, message: this.ml.minimumlengthrequired, trigger: 'blur' }
        ],
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
      // alert(isvalid)
      this.$refs.formItem2.validate((valid) => {
        isvalid = valid && isvalid
      })
      if (isvalid) {
        this.$refs.mySendButton.start()
        this.registerCustomer()
        // this.$root.eventHub.$emit('signalr.registerUser', this.registerString);
      } else {
        this.$Message.error(this.ml.formvalidationerror);
      }
      // this.$refs[name].validate((valid) => {
      //   if (valid) {
      //     this.$Message.success('Success!');
      //     this.$root.eventHub.$emit('signalr.registerUser', this.registerString);
      //   } else {
      //     this.$Message.error('Fail!');
      //   }
      // })
    },
    cancel() {
      this.show = false
      this.$emit('close')
    },
    openPrivacyPolicy() {
      this.$refs.myPrivacyPolicy.showPrivacyPolicy('privacypolicy')
    },
    openUseTerm() {
      this.$refs.myPrivacyPolicy.showPrivacyPolicy('useterm')
    },
    handleReset(name) {
      console.log(this.$refs[name]);
      console.log(JSON.stringify(this.formItem));
      this.formItem.lastname = '';
      this.$refs[name].resetFields();
    },
    registerCustomer() {
      console.log('registerCustomer')
      console.log(this.data.options.baseUrl)
      axios.post(this.data.options.requestUrl + 'method=registercustomer', this.registerString)
        .then((res) => {
          console.log('stop send')
          console.log(this)
          this.$refs.mySendButton.stop();
          console.log(res);
          if (res.data === 'Ok') {
            this.$Modal.success({
              title: this.ml.userregistrationemailsenttitle,
              content: this.ml.userregistrationemailsent,
              okText: this.ml.ok
            });
          } else if (res.data === 'emailaddressalreadyused') {
            this.$Modal.warning({
              content: this.ml.emailaddressalreadyused,
              okText: this.ml.ok
            });
          } else {
            this.$Modal.warning({
              title: this.ml.failed,
              content: this.ml.servererror,
              okText: this.ml.ok
            });
          }
        })
        .catch((error) => {
          console.log(error);
          this.$Modal.warning({
            title: 'ml.failed',
            content: 'ml.userregistrationemailsentfailed',
            okText: this.ml.ok
          });
        });
    },
    onRegisterUserConfirmedFromServerToWeb(userId, sessionId) {
      this.$refs.mySendButton.stop()
      if (userId === '-1') {
        this.$Modal.success({
          title: 'ml.failed',
          content: 'ml.userregistrationfailed',
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
          content: this.ml.userregistrationsuccess,
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
