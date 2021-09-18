<template>
    <div class="root">
        <Drawer
            :title="ml.monthmenu"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
         <i-form :model="formMonthMenu" label-position="top" ref="formMonthMenu" :rules="ruleValidate" >
        <form-item :label="ml.menunr" prop="menuNr">
                <i-input v-model="formMonthMenu.menuNr" ></i-input>
        </form-item>
        <form-item :label="ml.title" prop="title">
                <i-input v-model="formMonthMenu.title"></i-input>
        </form-item>
        <form-item :label="ml.pricelines">
            <i-input v-model="formMonthMenu.pricelines" type="textarea" :autosize="{minRows: 2,maxRows: 3}" placeholder=""></i-input>
        </form-item>
       <form-item :label="ml.foodlines">
            <i-input v-model="formMonthMenu.foodlines" type="textarea" :autosize="{minRows: 3,maxRows: 5}" placeholder=""></i-input>
        </form-item>
        <form-item :label="ml.endlines">
            <i-input v-model="formMonthMenu.endlines" type="textarea" :autosize="{minRows: 2,maxRows: 3}" placeholder=""></i-input>
        </form-item>
        <form-item :label="ml.separator">
                <i-input v-model="formMonthMenu.separator"></i-input>
        </form-item>
 </i-form>
          <modal
            ref="dialog"
            v-model="modalDeleteMonthMenu"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmDeleteMonthMenu"
            @on-cancel="cancelDeleteMonthMenu">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmdelete}}</span>
            </p>
          </modal>
          <div class="demo-drawer-footer">
              <!-- <sendButton ref="myDeleteButton" :text="ml.delete" :sendingText="ml.sending" :failedText="ml.deletefailed" @click="deleteMonthMenu()" style="margin-right: 8px"></sendButton> -->
              <!-- <Button style="margin-right: 8px" @click="deleteMonthMenu">{{ml.delete}}</Button> -->
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <sendButton ref="mySendButton" :text="ml.save" :sendingText="ml.sending" :failedText="ml.savefailed" @click="saveMonthMenu()"></sendButton>
              <!-- <Button type="primary" @click="saveMonthMenu">{{ml.save}}</Button> -->
          </div>
        </Drawer>
    </div>
</template>
<script>
  import sendButton from 'components/common/sendButton/sendButton'
  import axios from 'axios'
  export default {
    components: {
      sendButton
    },
    props: {
      ml: {},
      data: {}
      // monthMenu: {}
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
        modalDeleteMonthMenu: false,
        formMonthMenu: {
          title: '',
          menuNr: '',
          separator: '',
          pricelines: '',
          endlines: '',
          foodlines: '',
          sellerRid: ''
        },
        // formMonthMenu: {
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
          // name1: [
          //   { required: true, message: '*', trigger: 'blur' }
          // ]
        }
      }
    },
    created() {
      // this.$root.eventHub.$on('signalr.onSaveMonthMenu', this.onSaveMonthMenu)
      // this.$root.eventHub.$on('signalr.onDeleteMonthMenu', this.onDeleteMonthMenu)
      // this.$root.eventHub.$on('signalr.onDownLoadMonthMenu', this.onDownLoadMonthMenu)
    },
    mounted() {

    },
    computed: {
      dataString() {
        let requestString = ''
        requestString = JSON.stringify(this.formMonthMenu)
        return requestString
      }
    },
    methods: {
      showDraw(monthMenu) {
        console.log('show draw')
        this.show = true
        this.downloadMonthMenu()
      },
      close() {
        this.show = false
      },
      downloadMonthMenu() {
        console.log('downloadMonthMenu')
        var url = this.data.options.requestUrl + 'method=downloadmonthmenu&siterid=' + this.data.options.shopRid
        axios.get(url).then((res) => {
          console.log('get downloadMonthMenu ok')
          console.log(res)
          // statusText is empty in HTTP/2  https://stackoverflow.com/questions/41632077/why-is-the-statustext-of-my-xhr-empty
          // if (res.statusText === 'OK') {
          if (res.status === 200) {
            var monthMenuJson = res.data
            this.formMonthMenu.title = monthMenuJson.title
            this.formMonthMenu.menuNr = monthMenuJson.menuNr
            this.formMonthMenu.separator = monthMenuJson.separator
            this.formMonthMenu.pricelines = monthMenuJson.pricelines.join('\n')
            this.formMonthMenu.foodlines = monthMenuJson.foodlines.join('\n')
            this.formMonthMenu.endlines = monthMenuJson.endlines.join('\n')
            this.formMonthMenu.sellerRid = monthMenuJson.sellerRid
          }
        })
      },
      saveMonthMenu() {
        this.$refs.formMonthMenu.validate((valid) => {
          if (valid) {
            this.$refs.mySendButton.start()
            /* can't use this.formMonthMenu directly in axios.post(this.data.options.requestUrl + 'method=savemonthmenu', this.formMonthMenu)
              will get following error, signalr and menuweb in dev are in different domain.
              also must turn on browser extension "Allow CORS: Access-Control-Allow-Origin". this extension only needed for post, get not needed.
              Access to XMLHttpRequest at 'http://192.168.1.230:44337/RequestHandler.ashx?method=savemonthmenu' from origin 'http://localhost:9999' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource.
                configMonthMenu.vue?ea03:167 Error: Network Error
                    at createError (eval at <anonymous> (app.js:1110), <anonymous>:16:15)
                    at XMLHttpRequest.handleError (eval at <anonymous> (app.js:1092), <anonymous>:81:14) */
            axios.post(this.data.options.requestUrl + 'method=savemonthmenu', this.dataString)
              .then((res) => {
                console.log('stop send')
                console.log(this)
                this.$refs.mySendButton.stop();
                console.log(res);
                if (res.data === 'Ok') {
                  this.$Modal.success({
                    title: this.ml.success,
                    content: this.ml.savesuccessfully,
                    okText: this.ml.ok
                  });
                } else {
                  this.$Modal.warning({
                    title: this.failed,
                    content: this.ml.savefailed,
                    okText: this.ml.ok
                  });
                }
              })
              .catch((error) => {
                console.log(error);
                this.$Modal.warning({
                  title: this.ml.failed,
                  content: this.ml.savefailed,
                  okText: this.ml.ok
                });
              });
          } else {
            this.$Message.error(this.ml.formvalidationerror);
          }
        })
      },
      confirmDeleteMonthMenu() {
        this.$refs.myDeleteButton.start()
        this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'deleteMonthMenu', 'messageBody': this.formMonthMenu})
        this.modalDeleteMonthMenu = false
      },
      cancelDeleteMonthMenu() {
        this.modalDeleteMonthMenu = false
      },
      deleteMonthMenu() {
        this.modalDeleteMonthMenu = true
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
