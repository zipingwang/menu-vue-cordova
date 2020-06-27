<template>
    <div class="root">
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
            <img class="avatar" :src="imgUrl" > </img>
            <uploadFile :ml="ml" :data="data" :params = "uploadParam" @uploadSuccessed="onUploadSuccessed"></uploadFile>
            <Button shape="circle" icon="ios-close" @click="deleteImage"></Button>
            <!-- <uploadFile :ml="ml" :data="data" :params = "'method=menuimage&rid=' + formMenu.rid"></uploadFile> -->
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
           <modal
            ref="dialog"
            v-model="modalDeleteMenuImage"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteMenuImage"
            @on-cancel="cancelDeleteMenuImage">
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
  import uploadFile from 'components/common/uploadFile/uploadFile'

  export default {
    components: {
      sendButton,
      uploadFile
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
        modalDeleteMenuImage: false,
        formMenu: {},
        menuInDataJs: {},
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
        uploadCounter: 1,
        imgUploaded: false,
        imgDeleted: false
      }
    },
    created() {
      this.$root.eventHub.$on('signalr.onSaveMenu', this.onSaveMenu)
      this.$root.eventHub.$on('signalr.onDeleteMenu', this.onDeleteMenu)
      this.$root.eventHub.$on('signalr.onDeleteMenuImage', this.onDeleteMenuImage)
      // this.$root.eventHub.$on('signalr.onDownLoadMenu', this.onDownLoadMenu)
    },
    computed: {
      uploadParam() {
        return 'method=menuimage&rid=' + this.formMenu.rid + '&datetag=' + this.dateTag + '&count=' + this.uploadCounter
      },
      imgUrl() {
        if (this.imgDeleted) {
          return ''
        }

        if (this.imgUploaded === false) {
          return this.menuInDataJs.image
        } else {
          return 'static/gen/img/menu' + this.formMenu.rid + '' + this.dateTag + this.uploadCounter.toString() + '.jpg'
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
      showDraw(menu, menuGroupIdsWeb, menuInDataJs) {
        console.log('show draw')
        this.show = true
        this.menuGroupIdsWeb = menuGroupIdsWeb
        this.uploadCounter = menu.counter
        this.menuInDataJs = menuInDataJs
        console.log('menuInDataJs')
        console.log(this.menuInDataJs)
        console.log(menu)
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
      onDeleteMenuImage(messageBody) {
        console.log('onDeleteMenuImage in menu')
        this.$Message.success(this.ml.deletesuccessfully);
        this.imgDeleted = true
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
      },
      onUploadSuccessed() {
        this.imgDeleted = false
        this.imgUploaded = true
        this.formMenu.counter++
        console.log('onUploadSuccessed')
        this.uploadCounter++
      },
      deleteImage() {
        this.modalDeleteMenuImage = true
      },
      confirmDeleteMenuImage() {
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteMenuImage', 'messageBody': this.formMenu})
        this.modalDeleteMenuImage = false
      },
      cancelDeleteMenuImage() {
        this.modalDeleteMenuImage = false
      }
    }
  }
</script>
<style>
  .root {
    z-index: 101;
  }
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
