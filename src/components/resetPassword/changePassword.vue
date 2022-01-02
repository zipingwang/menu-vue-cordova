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

// .billcontainer {
//   display: flex;
//   flex-wrap: nowrap;
//   margin: 50px auto
// }

// .billcontent {
//   flex: 80%
// }

.columnpadding {
  flex : 10%
}

.ratingsWrapper
  height:350px;
  position: fixed;
  background-color: #00FF00;
  top: 50%;
  transform: translateY(-50%);
  width: 100%
  z-index 150
  background white
  overflow hidden

@media screen and (min-width: 350px)
  .ratingsWrapper
    width:350px;
    height:350px;
    position: fixed;
    background-color: #00FF00;
    top: 50%;
    margin-top: -175px;
    left: 50%;
    transform: translateX(-50%);
    z-index 150
    background white
    overflow hidden

</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper" v-show="isShow">
      <div class="billcontainer">
          <div class="billcontent">
            <card class="customer">
                <!-- <p slot="title">{{ml.resetpassword}}</p> -->
                <br/>
                 <i-form ref="formItem1" :model="formItem" :rules="ruleCustom" :label-width="100">
                 <!-- <i-form ref="formItem1" :model="formItem" :label-width="80"> -->
                    <form-item :label="ml.oldpassword" prop="oldPassword">
                        <i-input type="password" v-model="formItem.oldPassword"></i-input>
                    </form-item>
                    <form-item :label="ml.newpassword" prop="newPassword">
                        <i-input type="password" v-model="formItem.newPassword"></i-input>
                    </form-item>
                    <form-item :label="ml.confirmnewpassword" prop="newPasswordConfirm">
                        <i-input type="password" v-model="formItem.newPasswordConfirm"></i-input>
                    </form-item>
                    <form-item>
                        <sendButton ref="mySendButton" :ml="ml" :text="ml.changepassword" :timeout="30" :sendingText="ml.sending" :failedText="ml.communicationfailed"
                          @click="handleSubmit('formItem1')"></sendButton>
                    </form-item>
                </i-form>
            </card>
          </div>
      </div>
  </div>
</template>

<script>
import axios from 'axios'
import star from 'components/star/star'
import BScroll from 'better-scroll'
import sendButton from 'components/common/sendButton/sendButton'

export default {
  components: {
    sendButton
  },
  props: {
    data: {},
    ml: {},
    seller: {},
    isShow: true
  },
  data() {
    const validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error(this.ml.requiredfield));
      } else if (value.length < 6) {
        callback(new Error(this.ml.minimumlengthrequired));
      } else {
        if (this.formItem.newPasswordConfirm !== '') {
          // 对第二个密码框单独验证
          this.$refs.formItem1.validateField('newPasswordConfirm');
        }
        callback();
      }
    };
    const validatePassCheck = (rule, value, callback) => {
      if (value === '') {
        callback(new Error(this.ml.requiredfield));
      } else if (value !== this.formItem.newPassword) {
        callback(new Error(this.ml.passwordmismatch));
      } else {
        callback();
      }
    };
    return {
      address: this.seller.address,
      telefoon: this.seller.telefoon,
      openinghour: this.seller.openinghour,
      formItem: {
        oldPassword: '',
        newPassword: '',
        newPasswordConfirm: ''
      },
      ruleCustom: {
        oldPassword: [
          { validator: validatePass, trigger: 'blur' }
        ],
        newPassword: [
          { validator: validatePass, trigger: 'blur' }
        ],
        newPasswordConfirm: [
          { validator: validatePassCheck, trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    registerString() {
      let requestString = ''
      this.formItem['customerToken'] = this.data.options.customerToken
      requestString = JSON.stringify(this.formItem)
      return requestString
    }
  },
  mounted() {
  },
  watch: {
    isShow(newValue, oldValue) {
      this.formItem = {}
    }
  },
  methods: {
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
    requestChangePassword() {
      console.log('request change password from account info')
      axios.post(this.data.options.requestUrl + 'method=changepasswordinaccountinfo' + '&lnindex=' + this.data.currentlnindex + '&siterid=' + this.data.options.shopRid, this.registerString)
        .then((res) => {
          console.log('stop send')
          console.log(this)
          this.$refs.mySendButton.stop();
          console.log(res);
          if (res.data === 'Ok') {
            this.$Modal.success({
              title: this.ml.success,
              content: this.ml.resetpasswordsuccess,
              okText: this.ml.ok,
              onOk: () => { this.$emit('close') }
            });
          } else if (res.data === 'oldpasswordnotcorrect') {
            this.$Modal.warning({
              title: this.ml.failed,
              content: this.ml.oldpasswordnotcorrect,
              okText: this.ml.ok
            });
          } else {
            this.$Modal.warning({
              title: this.ml.failed,
              content: this.ml.resetpasswordemailsentfailed,
              okText: this.ml.ok
            });
          }
        })
        .catch((error) => {
          console.log(error);
          this.$Modal.warning({
            title: this.ml.failed,
            content: this.ml.resetpasswordemailsentfailed,
            okText: this.ml.ok
          });
        });
    }
  }
}

</script>
