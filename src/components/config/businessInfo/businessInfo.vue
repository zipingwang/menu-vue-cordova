<template>
    <div>
        <Drawer class="root"
            :title="ml.businessinfo"
            v-model="show"
            width="100%"
            :closable="false"
            :mask-closable="false"
            :styles="styles"
        >
        <i-form  ref="formSeller" :model="formSeller" label-position="top" :rules="ruleValidate">

          <form-item :label="ml.name" prop="name">
                  <i-input v-model="formSeller.name"></i-input>
          </form-item>

          <form-item :label="ml.description">
            <i-input v-model="formSeller.description" type="textarea" :autosize="{minRows: 2,maxRows: 7}" placeholder=""></i-input>
          </form-item>

          <form-item :label="ml.bulletin">
                  <i-input v-model="formSeller.bulletin"></i-input>
          </form-item>

          <form-item :label="ml.avatar">
             <img class="avatar" :src="imgUrl" > </img>
             <uploadFile :ml="ml" :data="data" :params = "uploadParam" @uploadSuccessed="onUploadSuccessed"></uploadFile>
          </form-item>

          <form-item :label="ml.openinghour">
            <i-input v-model="formSeller.openingHour" type="textarea" :autosize="{minRows: 2,maxRows: 7}" placeholder="b.v. maandag 12:00-14:00  18:00-22:00"></i-input>
          </form-item>

          <form-item :label="ml.address">
            <i-input v-model="formSeller.address" type="textarea" :autosize="{minRows: 2,maxRows: 7}" placeholder=""></i-input>
          </form-item>

          <form-item :label="ml.telephone">
                  <i-input v-model="formSeller.telephone"></i-input>
          </form-item>
          <form-item :label="ml.email">
              <i-input v-model="formSeller.email"></i-input>
          </form-item>
           <form-item :label="ml.taxnumber">
              <i-input v-model="formSeller.taxnumber"></i-input>
          </form-item>
          <form-item :label="ml.supportonlineorder">
            <i-switch v-model="formSeller.supportOnlineOrder" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
          </form-item>

        </i-form>
          <div class="demo-drawer-footer">
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveBusinessInfo"></sendButton>
              <!-- <Button type="primary" @click="saveBusinessInfo">{{ml.save}}</Button> -->
          </div>
        </Drawer>
    </div>
</template>
<script>
import sendButton from 'components/common/sendButton/sendButton'
import uploadFile from 'components/common/uploadFile/uploadFile'

export default {
  components: {
    sendButton,
    uploadFile
  },
  props: {
    ml: {},
    data: {},
    seller: {}
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
      formSeller: {
        rid: 0,
        name: '',
        description: '',
        bulletin: '',
        avatar: '',
        openingHour: '',
        address: '',
        telephone: '',
        supportOnlineOrder: true,
        counter: 1
      },
      ruleValidate: {
        name: [
            { required: true, message: this.ml.requiredfield, trigger: 'blur' }
        ]
      },
      uploadCounter: 1,
      imgUploaded: false
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onSaveBusinessInfo', this.onSaveBusinessInfo)
    this.$root.eventHub.$on('signalr.onDownLoadBusinessInfo', this.onDownLoadBusinessInfo)
  },
  computed: {
    uploadParam() {
      return 'method=selleravatar&datetag=' + this.dateTag + '&count=' + this.uploadCounter
    },
    imgUrl() {
      if (this.imgUploaded === false) {
        return this.seller.avatar
      } else {
        return 'static/gen/img/selleravatar' + this.dateTag + this.uploadCounter.toString() + '.jpg'
      }
    },
    dateTag() {
      let dt = new Date()
      let date = dt.getFullYear() + '.' + (dt.getMonth() + 1) + '.' + dt.getDate()
      console.log('dateTag')
      console.log(date)
      return date
    }
  },
  methods: {
    showDraw() {
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadBusinessInfo'})
      this.show = true
    },
    close() {
      this.$emit('closed')
      this.show = false
      console.log('emit closed')
    },
    saveBusinessInfo() {
      this.$refs.formSeller.validate((valid) => {
        if (valid) {
          this.$refs.mySendButton.start()
          this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveBusinessInfo', 'messageBody': this.formSeller})
        } else {
          this.$Message.error(this.ml.formvalidationerror);
        }
      })
    },
    onSaveBusinessInfo() {
      // alert('onSaveBusinessInfo')
      this.$refs.mySendButton.stop()
      this.$Message.success(this.ml.savesuccessfully);
    },
    onDownLoadBusinessInfo(messageBody) {
      // console.log('onDownloadBusinessInfo in businessinfo')
      // // alert('onDownloadBusinessInfo')
      // console.log(messageBody)
      // console.log(typeof messageBody)
      // console.log(messageBody.name)

      this.formSeller = messageBody
      console.log(this.formSeller.counter)
      this.uploadCounter = this.formSeller.counter
      this.$Message.success(this.ml.success);
    },
    onUploadSuccessed() {
      this.imgUploaded = true
      console.log('onUploadSuccessed')
      this.uploadCounter++
    }
  }
}
</script>
<style>
  .avatar{
    width: 57px;
    height: 57px;
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
</style>
