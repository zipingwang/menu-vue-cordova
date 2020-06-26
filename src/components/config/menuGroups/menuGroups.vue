<template>
    <div>
        <Drawer
            :title="ml.menugroup"
            v-model="show"
             :closable = "false"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div class="row">
              <div class="column" v-for="menuGroup in menuGroups">
                <div class="ricetableblock" @click="openMenuGroup(menuGroup)">
                      <div class="title">
                          {{menuGroup.name1}}
                      </div>
                      <div class="title">
                          {{menuGroup.displayOrder}}
                      </div>

                      <div class="endfilling"></div>
                  </div>
              </div>
            </div> <!--row -->
            <div class="demo-drawer-footer">
              <Button  type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <Button type="primary" @click="addNewMenuGroup">{{ml.add}}</Button>
          </div>
        </Drawer>
        <menuGroup ref="menuGroup" :ml="ml" :data="data"></menuGroup>
    </div>
</template>
<script>
import menuGroup from 'components/config/menuGroup/menuGroup'

export default {
  components: {
    menuGroup
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
      menuGroups: [],
      selectedMenuGroup: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onDownLoadMenuGroups', this.onDownLoadMenuGroups)
    this.$root.eventHub.$on('signalr.onDeleteMenuGroup', this.onDeleteMenuGroup)
    this.$root.eventHub.$on('signalr.onAddMenuGroup', this.onAddMenuGroup)
  },
  mounted() {

  },
  methods: {
    showDraw() {
      this.show = true
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadMenuGroups'})
    },
    onDownLoadMenuGroups(messageBody) {
      console.log('onDownLoadMenuGroups in menuGroups')
      // alert('onDownloadBusinessInfo')
      console.log(messageBody)
      console.log(typeof messageBody)
      this.menuGroups = messageBody
      this.$Message.success(this.ml.success);
    },
    onDeleteMenuGroup(messageBody) {
      console.log('onDeleteMenuGroup in menugroups')
      console.log(messageBody)
      for (var index = 0; index < this.menuGroups.length; index++) {
        if (this.menuGroups[index].rid === messageBody.rid) {
          this.menuGroups.splice(index, 1)
          console.log('menugroup deleted')
        }
      }
    },
    openMenuGroup(menuGroup) {
      console.log('open menugroup')
      this.selectedMenuGroup = menuGroup
      console.log(this.selectedMenuGroup)
      this.$refs.menuGroup.showDraw(menuGroup)
    },
    addNewMenuGroup() {
      var menuGroup = {}
      menuGroup.web = true
      menuGroup.displayOrder = 10
      this.$refs.menuGroup.showDraw(menuGroup)
    },
    onAddMenuGroup(newMenuGroup) {
      console.log('onAddMenuGroup')
      this.menuGroups.push(newMenuGroup)
    },
    close() {
      this.show = false
      this.$emit('closed')
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
