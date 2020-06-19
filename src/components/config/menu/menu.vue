<template>
    <div>
        <Drawer
            title="ml.menu"
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
              <input-number v-model="formMenuGroup.menuDisplayOrder" ></input-number>
          </form-item>
          <form-item :label="ml.menupreparationtime" prop="menuPreparationTime">
                  <input-number :max="200" :min="0" :step="0.5" v-model="formMenu.menuPreparationTime"></input-number>
          </form-item>
          <form-item :label="ml.menudisplayinweb" prop="menuDisplayInWeb">
              <i-switch v-model="formMenuGroup.menuDisplayInWeb" size="large">
                  <span slot="open">On</span>
                  <span slot="close">Off</span>
              </i-switch>
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
              <span>{{ml.askconfirmdeleteorder}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <Button style="margin-right: 8px" @click="deleteMenu">Delete</Button>
              <Button style="margin-right: 8px" @click="close">Cancel</Button>
              <Button type="primary" @click="saveMemuGroup">Submit</Button>
          </div>
        </Drawer>
    </div>
</template>
<script>
  export default {
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
          // displayOrder: [
          //   { required: false, message: '*', trigger: 'blur' }
          // ],
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
      showDraw(menu) {
        console.log('show draw')
        this.show = true
        this.formMenu = menu
      },
      test() {
        console.log('test')
      },
      close() {
        this.show = false
      },
      saveMemuGroup() {
        this.$refs.formMenu.validate((valid) => {
          if (valid) {
            this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'saveMenu', 'messageBody': this.formMenu})
          } else {
            alert('not valid')
            this.$Message.error('Fail!');
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
        alert('onSaveMenu')
        this.$Message.success('Saved');
        console.log(messageBody)
        console.log(typeof messageBody)
        console.log(messageBody.name1)
        console.log(messageBody.displayOrder)
        console.log(messageBody[0])
        this.formMenu.name1 = messageBody.name1
        console.log(this.formMenu)
        alert('done')
      },
      onDownLoadMenu(messageBody) {
        console.log('onDownLoadMenu in menu')
        // alert('onDownloadBusinessInfo')
        console.log(messageBody)
        console.log(typeof messageBody)
        // this.formMenu.name1 = messageBody.name1

        this.$Message.success('Success');
      },
      onDeleteMenu(messageBody) {
        console.log('onDeleteMenu in menu')
        this.$Message.success('Success');
        this.close()
      },
      deleteMenu() {
        this.modalDeleteMenu = true
      },
      handleUploadImageSuccess(res, file) {
        console.log('uploadimage success')
        file.url = this.avatarUrl
        this.$Message.success('Success');
      },
      handleFormatError(file) {
        this.$Notice.warning({
          title: 'The file format is incorrect',
          desc: 'File format of ' + file.name + ' is incorrect, please select jpg or png.'
        });
      },
      handleMaxSize(file) {
        this.$Notice.warning({
          title: 'Exceeding file size limit',
          desc: 'File  ' + file.name + ' is too large, no more than 4M.'
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
