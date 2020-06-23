<template>
    <div class="root">
        <Drawer
            :title="ml.menugroup"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form :model="formMenuGroup" label-position="top" ref="formMenuGroup" :rules="ruleValidate" >
        <form-item :label="ml.displayorder" prop="displayOrder">
                <input-number v-model="formMenuGroup.displayOrder" ></input-number>
        </form-item>
        <form-item :label="ml.name1" prop="name1">
                <i-input v-model="formMenuGroup.name1"></i-input>
        </form-item>
        <form-item :label="ml.name2">
                <i-input v-model="formMenuGroup.name2"></i-input>
        </form-item>
        <!-- <form-item :label="ml.menugroupimage">
                <i-input v-model="formMenuGroup.menuGroupImage"></i-input>
        </form-item> -->
        <form-item :label="ml.description">
                <i-input v-model="formMenuGroup.description"></i-input>
        </form-item>
        <form-item :label="ml.web">
            <i-switch v-model="formMenuGroup.web" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
        </form-item>
        <form-item :label="ml.name3">
                <i-input v-model="formMenuGroup.name3"></i-input>
        </form-item>
        <form-item :label="ml.name4">
                <i-input v-model="formMenuGroup.name4"></i-input>
        </form-item>
 </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteMenuGroup"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteMenuGroup"
            @on-cancel="cancelDeleteMenuGroup">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteMenuGroup" style="margin-right: 8px"></sendButton>
              <!-- <Button style="margin-right: 8px" @click="deleteMenuGroup">{{ml.delete}}</Button> -->
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveMemuGroup"></sendButton>
              <!-- <Button type="primary" @click="saveMemuGroup">{{ml.save}}</Button> -->
          </div>
        </Drawer>
    </div>
</template>
<script>
  import sendButton from 'components/common/sendButton/sendButton'
  export default {
    components: {
      sendButton
    },
    props: {
      ml: {},
      data: {}
      // menuGroup: {}
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
        modalDeleteMenuGroup: false,
        formMenuGroup: {},
        // formMenuGroup: {
        //   rid: 0,
        //   displayOrder: 1,
        //   name1: '',
        //   name2: '',
        //   description: '',
        //   web: true,
        //   name3: '',
        //   name4: ''
        // },
        ruleValidate: {
          // displayOrder: [
          //   { required: false, message: '*', trigger: 'blur' }
          // ],
          name1: [
            { required: true, message: '*', trigger: 'blur' }
          ]
        }
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveMenuGroup', this.onSaveMenuGroup)
      this.$root.eventHub.$on('signalr.onDeleteMenuGroup', this.onDeleteMenuGroup)
      // this.$root.eventHub.$on('signalr.onDownLoadMenuGroup', this.onDownLoadMenuGroup)
    },
    mounted() {

    },
    methods: {
      showDraw(menuGroup) {
        console.log('show draw')
        this.show = true
        this.formMenuGroup = menuGroup
      },
      close() {
        this.show = false
      },
      saveMemuGroup() {
        this.$refs.formMenuGroup.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveMenuGroup', 'messageBody': this.formMenuGroup})
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteMenuGroup() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteMenuGroup', 'messageBody': this.formMenuGroup})
        this.modalDeleteMenuGroup = false
      },
      cancelDeleteMenuGroup() {
        this.modalDeleteMenuGroup = false
      },
      onSaveMenuGroup(messageBody) {
        this.$refs.mySendButton.stop()
        this.$Message.success(this.ml.savesuccessfully);
        this.formMenuGroup = messageBody
        // console.log(messageBody)
        // console.log(typeof messageBody)
        // console.log(messageBody.name1)
        // console.log(messageBody.displayOrder)
        // console.log(messageBody[0])
        // this.formMenuGroup.name1 = messageBody.name1
        // console.log(this.formMenuGroup)
        // alert('done')
      },
      onDownLoadMenuGroup(messageBody) {
        console.log('onDownLoadMenuGroup in menuGroup')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formMenuGroup.name1 = messageBody.name1

        this.$Message.success(this.ml.success);
      },
      onDeleteMenuGroup(messageBody) {
        console.log('onDeleteMenuGroup in menuGroup')
        this.$refs.myDeleteButton.stop()
        this.$Message.success(this.ml.deletesuccessfully);
        this.close()
      },
      deleteMenuGroup() {
        this.modalDeleteMenuGroup = true
      },
      handleUploadImageSuccess(res, file) {
        console.log('uploadimage success')
        file.url = this.avatarUrl
        this.$Message.success(this.ml.success);
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
<style>
  .root {
    z-index: 101;
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
