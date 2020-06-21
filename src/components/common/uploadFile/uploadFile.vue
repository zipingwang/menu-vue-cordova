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
    data: {},
    params: ''
  },
  computed: {
    uploadImageUrl() {
      let url = data.options.uploadUrl
      return url + this.params
    }
  },
  methods: {
    handleUploadImageSuccess(res, file) {
      console.log('uploadimage success')
      this.$Message.success(this.ml.success);
      // file.url = this.avatarUrl
      // let avatarUrlTemp = this.avatarUrl
      // this.avatarUrl = ''
      // setTimeout(() => {
      //   this.avatarUrl = avatarUrlTemp
      // }, 1000);
      // this.$nextTick(() => {
      //   // this.avatarUrl = avatarUrlTemp
      // }
      // )
      // this.avatarUrl = avatarUrlTemp
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
