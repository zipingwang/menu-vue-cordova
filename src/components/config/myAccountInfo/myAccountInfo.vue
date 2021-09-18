<template>
    <div class="root">
        <Drawer
            :title="ml.myaccountinfo"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form ref="formMyAccountInfo" :model="formMyAccountInfo" :rules="ruleValidate" :label-width="120">
                 <!-- <i-form ref="formItem1" :model="formItem" :label-width="80"> -->
                    <FormItem :label="ml.persontitle" prop="title">
                      <i-select v-model="formMyAccountInfo.title" prop="title" placeholder="">
                        <i-option v-for="item in personTitleList" :value="item.value" :key="item.value">{{ item.label }}</i-option>
                      </i-select>
                    </FormItem>
                      <FormItem :label="ml.createuserfirstname" prop="firstName">
                        <i-input v-model="formMyAccountInfo.firstName"></i-input>
                    </FormItem>
                    <FormItem :label="ml.createuserlastname" prop="lastName">
                        <i-input v-model="formMyAccountInfo.lastName"></i-input>
                    </FormItem>
                     <FormItem :label="ml.createusertelephone" prop="telephone">
                        <i-input v-model="formMyAccountInfo.telephone" ></i-input>
                    </FormItem>
                     <FormItem :label="ml.createuseremail" prop="email">
                        <i-input v-model="formMyAccountInfo.email"></i-input>
                    </FormItem>
                     <FormItem :label="ml.street" prop="address">
                        <i-input v-model="formMyAccountInfo.address"></i-input>
                    </FormItem>
                     <FormItem :label="ml.postcode" prop="postcode">
                        <i-input v-model="formMyAccountInfo.postcode"></i-input>
                    </FormItem>
                     <FormItem :label="ml.city" prop="place">
                        <i-input v-model="formMyAccountInfo.place"></i-input>
                    </FormItem>
                     <FormItem>
                      <Button type="primary" @click="showBackdrop()">{{ml.changemypassword}}</Button>
                    </FormItem>
        </i-form>

          <modal
            ref="dialog"
            v-model="modalDeleteMyAccountInfo"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteMyAccountInfo"
            @on-cancel="cancelDeleteMyAccountInfo">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <!-- <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteMyAccountInfo()" style="margin-right: 8px"></sendButton> -->
              <!-- <Button style="margin-right: 8px" @click="deleteMyAccountInfo">{{ml.delete}}</Button> -->
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.cancel}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveMyAccountInfo()"></sendButton>
              <!-- <Button type="primary" @click="saveMyAccountInfo">{{ml.save}}</Button> -->
          </div>
        </Drawer>
         <login ref="myLogin" :seller="seller" :data="data" :ml="ml" ></login>
        <div class="changePasswordComponent" v-show="changingPassword">
          <backdrop :isShow="changingPassword" @click="closeChangePasswordBackDrop()"></backdrop>
          <div class="changePasswordWrapper">
            <changePassword ref="myResetPassword" :seller="seller" :data="data" :ml="ml" :isShow="changingPassword" @close="closeChangePasswordBackDrop()" ></changePassword>
          </div>
        </div>
    </div>
</template>
<script>
  import sendButton from 'components/common/sendButton/sendButton'
  import axios from 'axios'
  import login from 'components/login/login'
  import backdrop from 'components/backdrop/backdrop'
  import changePassword from 'components/resetPassword/changePassword'

  export default {
    components: {
      sendButton,
      login,
      backdrop,
      changePassword
    },
    props: {
      ml: {},
      data: {},
      seller: {}
      // myAccountInfo: {}
    },
    data () {
      return {
        styles: {
          height: 'calc(100% - 55px)',
          overflow: 'auto',
          paddingBottom: '53px',
          position: 'static'
        },
        show: this.visible,
        modalDeleteMyAccountInfo: false,
        changingPassword: false,
        formMyAccountInfo: {
          title: '-',
          firstName: '',
          lastName: '',
          // birthday: '',
          // birthdayday: '',
          // birthdaymonth: '',
          // birthdayyear: '',
          address: '',
          postcode: '',
          place: '',
          telephone: '',
          // mobile: '',
          email: ''
        },
        ruleValidate: {
          firstName: [
              { required: true, message: this.ml.requiredfield, trigger: 'blur' },
              { type: 'string', min: 2, message: this.ml.minimumlengthrequired, trigger: 'blur' }
          ],
          lastName: [
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
        },
        personTitleList: [
          {
            value: '-',
            label: '-'
          },
          {
            value: 'F',
            label: this.ml.womentitle
          },
          {
            value: 'M',
            label: this.ml.mantitle
          }
        ]
      }
    },
    created() {
      // this.$root.eventHub.$on('signalr.onSaveMyAccountInfo', this.onSaveMyAccountInfo)
      // this.$root.eventHub.$on('signalr.onDeleteMyAccountInfo', this.onDeleteMyAccountInfo)
      // this.$root.eventHub.$on('signalr.onDownLoadMyAccountInfo', this.onDownLoadMyAccountInfo)
      this.$root.eventHub.$on('login.loggedOut', this.onLoggedOut)
    },
    mounted() {

    },
    computed: {
      dataString() {
        let requestString = ''
        this.formMyAccountInfo.customerToken = this.data.options.customerToken
        requestString = JSON.stringify(this.formMyAccountInfo)
        return requestString
      }
    },
    methods: {
      showDraw(myAccountInfo) {
        console.log('show draw')
        this.show = true
        this.downloadMyAccountInfo()
      },
      close() {
        this.show = false
      },
      showBackdrop() {
        this.changingPassword = true
      },
      closeChangePasswordBackDrop() {
        console.log('closeChangePasswordBackDrop')
        this.changingPassword = false
      },
      login() {
        this.$refs.myLogin.showlogin()
        this.sideDrawVisible = false
      },
      onLoggedOut() {
        this.formMyAccountInfo = {}
      },
      downloadMyAccountInfo() {
        console.log('downloadMyAccountInfo')
        var url = this.data.options.requestUrl + 'method=downloadmyaccountinfo&customerToken=' + this.data.options.customerToken
        axios.get(url).then((res) => {
          console.log('get downloadMyAccountInfo ok')
          console.log(res)
          // alert(res)
          // statusText is empty in HTTP/2  https://stackoverflow.com/questions/41632077/why-is-the-statustext-of-my-xhr-empty
          // if (res.statusText === 'OK') {
          console.log(typeof (res.status))
          console.log(res.status)
          if (res.status === 200) {
            var myAccountInfoJson = res.data
            if (myAccountInfoJson.title === '') {
              myAccountInfoJson.title = '-'
            }
            this.formMyAccountInfo = myAccountInfoJson
            // this.formMyAccountInfo.title = myAccountInfoJson.title
            // this.formMyAccountInfo.menuNr = myAccountInfoJson.menuNr
            // this.formMyAccountInfo.separator = myAccountInfoJson.separator
            // this.formMyAccountInfo.pricelines = myAccountInfoJson.pricelines.join('\n')
            // this.formMyAccountInfo.foodlines = myAccountInfoJson.foodlines.join('\n')
            // this.formMyAccountInfo.endlines = myAccountInfoJson.endlines.join('\n')
            // this.formMyAccountInfo.sellerRid = myAccountInfoJson.sellerRid
          }
        })
      },
      saveMyAccountInfo() {
        this.$refs.formMyAccountInfo.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            /* can't use this.formMyAccountInfo directly in axios.post(this.data.options.requestUrl + 'method=savemonthmenu', this.formMyAccountInfo)
              will get following error, signalr and menuweb in dev are in different domain.
              also must turn on browser extension "Allow CORS: Access-Control-Allow-Origin". this extension only needed for post, get not needed.
              Access to XMLHttpRequest at 'http://192.168.1.230:44337/RequestHandler.ashx?method=savemonthmenu' from origin 'http://localhost:9999' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource.
                configMyAccountInfo.vue?ea03:167 Error: Network Error
                    at createError (eval at <anonymous> (app.js:1110), <anonymous>:16:15)
                    at XMLHttpRequest.handleError (eval at <anonymous> (app.js:1092), <anonymous>:81:14) */
            axios.post(this.data.options.requestUrl + 'method=savemyaccountinfo', this.dataString)
              .then((res) => {
                console.log('stop send')
                console.log(this)
                this.$refs.mySendButton.stop();
                console.log(res);
                if (res.data === 'Ok') {
                  this.$Modal.success({
                    title: this.ml.success,
                    content: this.ml.savesuccessfully,
                    okText: this.ml.ok
                  });
                } else {
                  this.$Modal.warning({
                    title: this.failed,
                    content: this.ml.savefailed,
                    okText: this.ml.ok
                  });
                }
              })
              .catch((error) => {
                console.log(error);
                this.$Modal.warning({
                  title: this.ml.failed,
                  content: this.ml.savefailed,
                  okText: this.ml.ok
                });
              });
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteMyAccountInfo() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteMyAccountInfo', 'messageBody': this.formMyAccountInfo})
        this.modalDeleteMyAccountInfo = false
      },
      cancelDeleteMyAccountInfo() {
        this.modalDeleteMyAccountInfo = false
      },
      deleteMyAccountInfo() {
        this.modalDeleteMyAccountInfo = true
      }
    }
  }
</script>
<style lang="stylus" scoped>
  .root {
    z-index: 1001; /* important, draw index is 1000(default) */
  }
  .demo-drawer-footer{
    width: 100%;
    position: absolute;
    bottom: 0;
    left: 0;
    border-top: 1px solid #e8e8e8;
    padding: 10px 16px;
    text-align: right;
    background: #fff;
  }
  .changeMyPasswordButton {
    margin: auto;
  }
  .changePasswordComponent
    position fixed
    top 0
    bottom 0
    left 0
    right 0
  .changePasswordWrapper
    position absolute
    width 100%
    height 100%
</style>
