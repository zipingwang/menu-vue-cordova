<template>
    <div class="root">
        <Drawer
            :title="ml.customer"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form :model="formCustomer" label-position="top" ref="formCustomer" :rules="ruleValidate" >
        <form-item :label="ml.firstName" prop="firstName">
                <i-input v-model="formCustomer.firstName" ></i-input>
        </form-item>
        <form-item :label="ml.lastName" prop="lastName">
                <i-input v-model="formCustomer.lastName"></i-input>
        </form-item>
        <form-item :label="ml.email">
                <i-input v-model="formCustomer.email"></i-input>
        </form-item>
        <!-- <form-item :label="ml.customerimage">
                <i-input v-model="formCustomer.customerImage"></i-input>
        </form-item> -->
        <form-item :label="ml.telephone">
                <i-input v-model="formCustomer.telephone"></i-input>
        </form-item>
        <form-item :label="ml.isapproved">
            <i-switch v-model="formCustomer.isApproved" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
        </form-item>
         <form-item :label="ml.islockedout">
            <i-switch v-model="formCustomer.isLockedOut" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
        </form-item>
 </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteCustomer"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteCustomer"
            @on-cancel="cancelDeleteCustomer">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteCustomer" style="margin-right: 8px"></sendButton>
              <!-- <Button style="margin-right: 8px" @click="deleteCustomer">{{ml.delete}}</Button> -->
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
      // customer: {}
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
        modalDeleteCustomer: false,
        formCustomer: {},
        // formCustomer: {
        //   rid: 0,
        //   displayOrder: 1,
        //   lastName: '',
        //   email: '',
        //   telephone: '',
        //   web: true,
        //   name3: '',
        //   name4: ''
        // },
        ruleValidate: {
          // displayOrder: [
          //   { required: false, message: '*', trigger: 'blur' }
          // ],
          lastName: [
            { required: true, message: '*', trigger: 'blur' }
          ]
        }
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveCustomer', this.onSaveCustomer)
      this.$root.eventHub.$on('signalr.onDeleteCustomer', this.onDeleteCustomer)
      // this.$root.eventHub.$on('signalr.onDownLoadCustomer', this.onDownLoadCustomer)
    },
    mounted() {

    },
    methods: {
      showDraw(customer) {
        console.log('show draw')
        this.show = true
        this.formCustomer = customer
      },
      close() {
        this.show = false
      },
      saveMemuGroup() {
        this.$refs.formCustomer.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveCustomer', 'messageBody': this.formCustomer})
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteCustomer() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteCustomer', 'messageBody': this.formCustomer})
        this.modalDeleteCustomer = false
      },
      cancelDeleteCustomer() {
        this.modalDeleteCustomer = false
      },
      onSaveCustomer(messageBody) {
        this.$refs.mySendButton.stop()
        this.$Message.success(this.ml.savesuccessfully);
        this.formCustomer = messageBody
        // console.log(messageBody)
        // console.log(typeof messageBody)
        // console.log(messageBody.lastName)
        // console.log(messageBody.displayOrder)
        // console.log(messageBody[0])
        // this.formCustomer.lastName = messageBody.lastName
        // console.log(this.formCustomer)
        // alert('done')
      },
      onDownLoadCustomer(messageBody) {
        console.log('onDownLoadCustomer in customer')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formCustomer.lastName = messageBody.lastName

        this.$Message.success(this.ml.success);
      },
      onDeleteCustomer(messageBody) {
        console.log('onDeleteCustomer in customer')
        this.$refs.myDeleteButton.stop()
        this.$Message.success(this.ml.deletesuccessfully);
        this.close()
      },
      deleteCustomer() {
        this.modalDeleteCustomer = true
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
