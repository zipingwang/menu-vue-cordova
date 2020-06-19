<template>
    <div>
        <Drawer
            title="ml.menu"
            v-model="show"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div class="row">
              <div class="column" v-for="menu in menus">
                <div class="ricetableblock" @click="openMenu(menu)">
                      <div class="title">
                          {{menu.menuNr}}
                      </div>
                      <div class="title">
                          {{menu.name1}}
                      </div>
                      <div class="title">
                          {{menu.displayOrder}}
                      </div>
                      <div class="endfilling"></div>
                  </div>
              </div>
            </div> <!--row -->
            <div class="demo-drawer-footer">
              <Button style="margin-right: 8px" @click="close">Close</Button>
              <Button type="primary" @click="addNewMenu">Add</Button>
          </div>
        </Drawer>
        <menu ref="menu" :ml="ml" :data="data"></menu>
    </div>
</template>
<script>
import menu from 'components/config/menu/menu'

export default {
  components: {
    menu
  },
  props: {
    ml: {},
    data: {}
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
      menus: [],
      selectedMenu: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onDownLoadMenus', this.onDownLoadMenus)
    this.$root.eventHub.$on('signalr.onDeleteMenu', this.onDeleteMenu)
    this.$root.eventHub.$on('signalr.onAddMenu', this.onAddMenu)
  },
  mounted() {

  },
  methods: {
    showDraw() {
      this.show = true
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadMenus'})
    },
    onDownLoadMenus(messageBody) {
      console.log('onDownLoadMenus in menus')
      // alert('onDownloadBusinessInfo')
      console.log(messageBody)
      console.log(typeof messageBody)
      this.menus = messageBody
      this.$Message.success('Success');
    },
    onDeleteMenu(messageBody) {
      console.log('onDeleteMenu in menus')
      console.log(messageBody)
      for (var index = 0; index < this.menus.length; index++) {
        if (this.menus[index].rid === messageBody.rid) {
          this.menus.splice(index, 1)
          console.log('menu deleted')
        }
      }
    },
    openMenu(menu) {
      console.log('open menu')
      this.selectedMenu = menu
      console.log(this.selectedMenu)
      // this.$refs.menu.showDraw(menu)
      console.log(this.$refs.menu)
      this.$refs.menu.test()
    },
    addNewMenu() {
      var menu = {}
      menu.web = true
      menu.displayOrder = 10
      this.$refs.menu.showDraw(menu)
    },
    onAddMenu(newMenu) {
      console.log('onAddMenu')
      this.menus.push(newMenu)
    },
    close() {
      this.show = false
    }
  }
}
</script>
<style lang="stylus" scoped>
/* Container for flexboxes */
.row {
  display: flex;
  flex-wrap: wrap;
}

/* Create four equal columns */
/* maximum 4 column, if screen width is not enough, it will display less column, e.g. 3 or 2 or 1, new row will made. */
.column {
  flex: 25%;
  padding: 20px;
}
/* On screens that are 992px wide or less, go from four columns to two columns */
@media screen and (max-width: 992px) {
  .column {
    flex: 50%;
  }
}

/* On screens that are 600px wide or less, make the columns stack on top of each other instead of next to each other */
@media screen and (max-width: 600px) {
  .row {
    flex-direction: column;
  }
}

.row
  .column
    text-align center
    color rgb(77,85,93)
    .ricetableblock
      border 1px solid #f3f5f7
      border-radius: 10px
      height: 100%
      box-sizing border-box
      .title
        font-size 24px
        line-height 24px
        margin-top 20px
      .endfilling
        height 20px
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
