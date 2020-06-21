<template>
    <div>
        <Drawer
            :title="ml.menu"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
        <i-form :model="formMenu" label-position="top" ref="formMenu" :rules="ruleValidate" >
          <form-item :label="ml.menunr" prop="menuNr">
                  <i-input v-model="formMenu.menuNr"></i-input>
          </form-item>
          <form-item :label="ml.menuprice" prop="menuPrice">
              <input-number :max="200" :min="0" :step="0.5" v-model="formMenu.menuPrice"></input-number>
          </form-item>
          <form-item :label="ml.menutakeawayprice" prop="menuTakeAwayPrice">
                  <input-number :max="200" :min="0" :step="0.5" v-model="formMenu.menuTakeAwayPrice"></input-number>
          </form-item>
          <form-item :label="ml.menudisplayorder" prop="menuDisplayOrder">
              <input-number v-model="formMenu.menuDisplayOrder" ></input-number>
          </form-item>
          <form-item :label="ml.menupreparationtime" prop="menuPreparationTime">
                  <input-number :max="200" :min="0" :step="0.5" v-model="formMenu.menuPreparationTime"></input-number>
          </form-item>
          <form-item :label="ml.menudisplayinweb" prop="menuDisplayInWeb">
              <i-switch v-model="formMenu.menuDisplayInWeb" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
          </form-item>
           <form-item :label="ml.menugroup" prop="menuGroup">
               <i-select v-model="formMenu.menuGroupRid" style="width:200px">
                  <i-option v-for="item in menuGroupIdsWeb" :value="item.rid" :key="item.rid">{{item.name1}}</i-option>
              </i-select>
           </form-item>
          <form-item :label="ml.name1" prop="name1">
                  <i-input v-model="formMenu.name1"></i-input>
          </form-item>
          <form-item :label="ml.name2" prop="name2">
                  <i-input v-model="formMenu.name2"></i-input>
          </form-item>
          <form-item :label="ml.menuimage" prop="menuImage">
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
          <form-item :label="ml.description" prop="description">
                  <i-input v-model="formMenu.description"></i-input>
          </form-item>
          <form-item :label="ml.name3" prop="name3">
                  <i-input v-model="formMenu.name3"></i-input>
          </form-item>
          <form-item :label="ml.name4" prop="name4">
                  <i-input v-model="formMenu.name4"></i-input>
          </form-item>
        </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteMenu"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteMenu"
            @on-cancel="cancelDeleteMenu">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <Button  type="primary" style="margin-right: 8px" @click="deleteMenu">{{ml.delete}}</Button>
              <Button  type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <!-- <Button type="primary" @click="saveMemuGroup">{{ml.save}}</Button> -->
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveMenu"></sendButton>
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
      // menu: {}
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
        modalDeleteMenu: false,
        formMenu: {},
        menuGroupIdsWeb: [],
        // formMenu: {
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
          menuNr: [
              { required: true, message: '*', trigger: 'blur' }
          ],
          name1: [
              { required: true, message: '*', trigger: 'blur' }
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
      this.$root.eventHub.$on('signalr.onSaveMenu', this.onSaveMenu)
      this.$root.eventHub.$on('signalr.onDeleteMenu', this.onDeleteMenu)
      // this.$root.eventHub.$on('signalr.onDownLoadMenu', this.onDownLoadMenu)
    },
    mounted() {

    },
    methods: {
      showDraw(menu, menuGroupIdsWeb) {
        console.log('show draw')
        this.show = true
        this.menuGroupIdsWeb = menuGroupIdsWeb
        console.log(menu.menuGroupRid)
        this.formMenu = menu
      },
      close() {
        this.show = false
      },
      saveMenu() {
        this.$refs.formMenu.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveMenu', 'messageBody': this.formMenu})
          } else {
            alert('not valid')
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteMenu() {
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteMenu', 'messageBody': this.formMenu})
        this.modalDeleteMenu = false
      },
      cancelDeleteMenu() {
        this.modalDeleteMenu = false
      },
      onSaveMenu(messageBody) {
        this.$refs.mySendButton.stop()
        // alert('onSaveMenu')
        this.formMenu = messageBody
        this.$Message.success(this.ml.savesuccessfully);
        // console.log(messageBody)
        // console.log(typeof messageBody)
        // console.log(messageBody.name1)
        // console.log(messageBody.displayOrder)
        // console.log(messageBody[0])
        // this.formMenu.name1 = messageBody.name1
        // console.log(this.formMenu)
        // alert('done')
      },
      onDownLoadMenu(messageBody) {
        console.log('onDownLoadMenu in menu')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formMenu.name1 = messageBody.name1

        this.$Message.success(this.ml.success);
      },
      onDeleteMenu(messageBody) {
        console.log('onDeleteMenu in menu')
        this.$Message.success(this.ml.deletesuccessfully);
        this.close()
      },
      deleteMenu() {
        this.modalDeleteMenu = true
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
