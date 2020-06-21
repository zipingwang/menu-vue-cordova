<template lang="html">
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
</template>

<script>

export default {
  props: {
    ml: {},
    text: '',
    sendingText: {
      default: ml.sending,
      type: String
    },
    failedTitle: '',
    failedText: '',
    timeout: { /* if want to set default value, need to assign object */
      default: 9, /* in seconds */
      type: Number
    }
  },
  data() {
    return {
      busyWithSending: false,
      startTime: {},
      mySendingTimer: {}
    }
  },
  computed: {
    uploadImageUrl() {
      let url = 'http://localhost:44337/'
      // let url = data.options.baseUrl
      return url + 'RequestHandler.ashx?method=selleravatar'
    }
  },
  methods: {
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
        desc: this.ml.fileformatnotcorrect
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: '',
        desc: this.ml.filesizetoolarge
      });
    }
  }
}

</script>

<style lang="stylus" scoped>
</style>
