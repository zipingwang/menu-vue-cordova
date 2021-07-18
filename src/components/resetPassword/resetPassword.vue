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

.billcontainer {
  display: flex;
  flex-wrap: nowrap;
  margin: 50px auto
}

.billcontent {
  flex: 80%
}

.columnpadding {
  flex : 10%
}

.ratingsWrapper
  position: absolute
  top: 174px
  bottom: 0
  left: 0
  width: 100%
  overflow: hidden

</style>

<template lang="html">
  <div class="ratingsWrapper" ref="ratingsWrapper">
      <div class="billcontainer">
          <div class="columnpadding"></div>
          <div class="billcontent">
            <card class="customer">
                <p slot="title">{{ml.resetpassword}}reset password</p>
                <br/>

                 <i-form ref="formItem1" :model="formItem" :rules="ruleCustom" :label-width="100">
                 <!-- <i-form ref="formItem1" :model="formItem" :label-width="80"> -->
                    <form-item :label="ml.newpassword" prop="password">
                        <i-input type="password" v-model="formItem.password"></i-input>
                    </form-item>
                    <form-item :label="ml.confirmpassword" prop="passwordconfirm">
                        <i-input type="password" v-model="formItem.passwordconfirm"></i-input>
                    </form-item>
                    <form-item>
                        <sendButton ref="mySendButton" :ml="ml" :text="ml.resetpassword" :sendingText="ml.sending" :failedText="ml.communicationfailed"
                          @click="handleSubmit('formItem1')" :timeout="15"></sendButton>
                    </form-item>
                </i-form>
            </card>
          </div>
          <div class="columnpadding"></div>
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
    seller: {}
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
          this.$refs.formItem1.validateField('passwordconfirm');
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
      address: this.seller.address,
      telefoon: this.seller.telefoon,
      openinghour: this.seller.openinghour,
      formItem: {
        password: '',
        passwordconfirm: ''
      },
      ruleCustom: {
        password: [
          { validator: validatePass, trigger: 'blur' }
        ],
        passwordconfirm: [
          { validator: validatePassCheck, trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    registerString() {
      let requestString = ''
      this.formItem['requesturl'] = this.data.options.requestUrl // dynamic insert, not put in url, in post body url auto encoded
      this.formItem['baseurl'] = this.data.options.baseUrl
      this.formItem['changepasswordtoken'] = this.data.options.changepasswordtoken
      requestString = JSON.stringify(this.formItem)
      return requestString
    }
  },
  mounted() {
  },
  watch: {
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
      console.log('request change password')
      axios.post(this.data.options.requestUrl + 'method=changepassword' + '&lnindex=' + this.data.currentlnindex + '&siterid=' + this.data.options.shopRid, this.registerString)
        .then((res) => {
          console.log('stop send')
          console.log(this)
          this.$refs.mySendButton.stop();
          console.log(res);
          if (res.data === 'Ok') {
            this.$Modal.success({
              title: this.ml.success,
              content: this.ml.resetpasswordsuccess,
              okText: this.ml.ok
            });
          } else if (res.data === 'ChangePasswordLinkNoMoreValid') {
            this.$Modal.warning({
              title: 'ml.failed',
              content: 'ml.changepasswordlinknomorevalid',
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
    }
  }
}

</script>
