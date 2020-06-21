<template>
    <div>
        <Drawer
            :title="ml.businessinfo"
            v-model="show"
            width="100%"
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
              <Upload
                :action= "uploadImageUrl"
                :on-success="handleUploadImageSuccess"
                :on-format-error="handleFormatError"
                :on-exceeded-size="handleMaxSize"
                :format="['jpg','jpeg','png']"
                :max-size="4096"
              >
                <Button icon="ios-cloud-upload-outline">Upload files</Button>
              </Upload>
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

          <form-item :label="ml.supportonlineorder">
            <i-switch v-model="formSeller.supportOnlineOrder" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
          </form-item>

        </i-form>
          <div class="demo-drawer-footer">
              <Button style="margin-right: 8px" @click="close">{{ml.cancel}}</Button>
              <Button type="primary" @click="saveBusinessInfo">{{ml.save}}</Button>
          </div>
        </Drawer>
    </div>
</template>
<script>
  export default {
    props: {
      ml: {},
      data: {}
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
          name: '',
          description: '',
          bulletin: '',
          avatar: '',
          openingHour: '',
          address: '',
          telephone: '',
          supportOnlineOrder: true
        },
        ruleValidate: {
          name: [
              { required: true, message: 'The name cannot be empty', trigger: 'blur' }
          ]
        },
        // avatarUrl: data.options.baseUrl + '/static/img/avarta.jpg'
        avatarUrl: 'http://localhost:44337/' + '/static/img/avatar.jpg'
      }
    },
    computed: {
      uploadImageUrl() {
        let url = 'http://localhost:44337/'
        // let url = data.options.baseUrl
        return url + 'RequestHandler.ashx?method=selleravatar'
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveBusinessInfo', this.onSaveBusinessInfo)
      this.$root.eventHub.$on('signalr.onDownLoadBusinessInfo', this.onDownLoadBusinessInfo)
    },
    mounted() {

    },
    methods: {
      showDraw() {
        alert(this.uploadImageUrl)
        this.show = true
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadBusinessInfo'})
      },
      close() {
        this.show = false
      },
      saveBusinessInfo() {
        this.$refs.formSeller.validate((valid) => {
          if (valid) {
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveBusinessInfo', 'messageBody': this.formSeller})
          } else {
            this.$Message.error('{{ml.formvalidationerror}}');
          }
        })
      },
      onSaveBusinessInfo() {
        // alert('onSaveBusinessInfo')
        this.$Message.success('{{ml.savesuccessfully}}');
      },
      onDownLoadBusinessInfo(messageBody) {
        console.log('onDownloadBusinessInfo in businessinfo')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        console.log(messageBody.name)

        this.formSeller = messageBody

        this.$Message.success('{{ml.success}}');
      },
      handleUploadImageSuccess(res, file) {
        console.log('uploadimage success')
        file.url = this.avatarUrl
        let avatarUrlTemp = this.avatarUrl
        this.avatarUrl = ''
        setTimeout(() => {
          this.avatarUrl = avatarUrlTemp
        }, 1000);
        this.$nextTick(() => {
          // this.avatarUrl = avatarUrlTemp
        }
        )
        this.avatarUrl = avatarUrlTemp
      },
      handleFormatError(file) {
        this.$Notice.warning({
          title: '',
          desc: '{{ml.fileformatnotcorrect}}'
        });
      },
      handleMaxSize(file) {
        this.$Notice.warning({
          title: '',
          desc: '{{ml.filesizetoolarge}}'
        });
      }
    }
  }
</script>
<style>
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
