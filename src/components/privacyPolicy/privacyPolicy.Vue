<template>
    <div class="root">
        <Drawer
            title=" "
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div>
            <p style="white-space: pre-line">{{privacyPolicyText}}</p>
          </div>
          <div class="demo-drawer-footer">
              <Button type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
          </div>
        </Drawer>
    </div>
</template>
<script>
  import axios from 'axios'
  export default {
    props: {
      seller: {},
      data: {},
      ml: {}
    },
    data () {
      return {
        styles: {
          height: 'calc(100% - 55px)',
          overflow: 'auto',
          paddingBottom: '53px',
          position: 'static'
        },
        privacyPolicyText: '1234',
        show: this.visible
      }
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
      showPrivacyPolicy() {
        this.show = true
        this.downLoadPrivacyPolicy()
      },
      downLoadPrivacyPolicy() {
        let url = data.options.uploadUrl + 'method=privacypolicy&lnindex=' + this.data.currentlnindex + '&siterid=' + this.data.options.shopRid
        console.log(this.data)
        console.log(this.seller)
        console.log(this.ml)
        console.log(url)
        axios.get(url).then((res) => {
          console.log('get from customerdaga.json')
          console.log(res)
          this.privacyPolicyText = res.data
        })
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
