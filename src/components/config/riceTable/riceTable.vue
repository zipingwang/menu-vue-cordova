<template>
    <div class="root">
        <Drawer
            :title="ml.ricetable"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form :model="formRiceTable" label-position="top" ref="formRiceTable" :rules="ruleValidate" >
        <form-item :label="ml.displayorder" prop="displayOrder">
                <input-number v-model="formRiceTable.displayOrder" ></input-number>
        </form-item>
        <form-item :label="ml.name" prop="title">
                <i-input v-model="formRiceTable.title"></i-input>
        </form-item>
        <form-item :label="ml.pricelines">
                <i-input v-model="formRiceTable.priceLine"  type="textarea" :autosize="{minRows: 2,maxRows: 3}"></i-input>
        </form-item>
        <form-item :label="ml.foodlines">
                <i-input v-model="formRiceTable.foodLines"  type="textarea" :autosize="{minRows: 2,maxRows: 10}"></i-input>
        </form-item>
        <form-item :label="ml.separator">
                <i-input v-model="formRiceTable.separator"></i-input>
        </form-item>
        <form-item :label="ml.endlines">
                <i-input v-model="formRiceTable.endLine"  type="textarea" :autosize="{minRows: 2,maxRows: 3}"></i-input>
        </form-item>
         <form-item :label="ml.web">
            <i-switch v-model="formRiceTable.web" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
        </form-item>
         <form-item :label="ml.menunr">
                <i-input v-model="formRiceTable.menuNr"></i-input>
        </form-item>
 </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteRiceTable"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteRiceTable"
            @on-cancel="cancelDeleteRiceTable">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteRiceTable" style="margin-right: 8px"></sendButton>
              <!-- <Button style="margin-right: 8px" @click="deleteRiceTable">{{ml.delete}}</Button> -->
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveRiceTable"></sendButton>
              <!-- <Button type="primary" @click="saveRiceTable">{{ml.save}}</Button> -->
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
      // riceTable: {}
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
        modalDeleteRiceTable: false,
        formRiceTable: {},
        // formRiceTable: {
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
          title: [
            { required: true, message: '*', trigger: 'blur' }
          ]
        }
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveRiceTable', this.onSaveRiceTable)
      this.$root.eventHub.$on('signalr.onDeleteRiceTable', this.onDeleteRiceTable)
      // this.$root.eventHub.$on('signalr.onDownLoadRiceTable', this.onDownLoadRiceTable)
    },
    mounted() {

    },
    methods: {
      showDraw(riceTable) {
        console.log('show draw')
        this.show = true
        this.formRiceTable = riceTable
      },
      close() {
        this.show = false
      },
      saveRiceTable() {
        this.$refs.formRiceTable.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveRiceTable', 'messageBody': this.formRiceTable})
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteRiceTable() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteRiceTable', 'messageBody': this.formRiceTable})
        this.modalDeleteRiceTable = false
      },
      cancelDeleteRiceTable() {
        this.modalDeleteRiceTable = false
      },
      onSaveRiceTable(messageBody) {
        this.$refs.mySendButton.stop()
        this.$Message.success(this.ml.savesuccessfully);
        this.formRiceTable = messageBody
        // console.log(messageBody)
        // console.log(typeof messageBody)
        // console.log(messageBody.name1)
        // console.log(messageBody.displayOrder)
        // console.log(messageBody[0])
        // this.formRiceTable.name1 = messageBody.name1
        // console.log(this.formRiceTable)
        // alert('done')
      },
      onDownLoadRiceTable(messageBody) {
        console.log('onDownLoadRiceTable in riceTable')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formRiceTable.name1 = messageBody.name1

        this.$Message.success(this.ml.success);
      },
      onDeleteRiceTable(messageBody) {
        console.log('onDeleteRiceTable in riceTable')
        this.$refs.myDeleteButton.stop()
        this.$Message.success(this.ml.deletesuccessfully);
        this.close()
      },
      deleteRiceTable() {
        this.modalDeleteRiceTable = true
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
