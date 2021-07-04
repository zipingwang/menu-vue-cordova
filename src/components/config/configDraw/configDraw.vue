<template>
    <div>
       <!-- <icon type="ios-keypad" size="20" class="config" v-if="data.options.isAdmin === '1'" @click="showConfig" /> -->
        <!-- <icon type="ios-keypad" size="20" class="config" @click="showConfig" /> -->
        <drawer ref="menuDrawer" :title="ml.config" placement="right" :closable="true"  v-model="configVisible">
        <!-- <drawer ref="menuDrawer" :title="ml.config" placement="right" :closable="true" v-if="data.options.isAdmin === '1'" v-model="configVisible"> -->
          <p class="drawbutton"><i-button @click="configSeller" type="primary">{{ml.seller}}</i-button></p>
          <p class="drawbutton"><i-button @click="configMenuGroups" type="primary">{{ml.menugroup}}</i-button></p>
          <p class="drawbutton"><i-button @click="configMenus" type="primary">{{ml.menu}}</i-button></p>
          <!-- <p class="drawbutton"><i-button @click="configRiceTable" type="primary">{{ml.ricetable}}</i-button></p> -->
          <p class="drawbutton"><i-button @click="configOpeninghour" type="primary">{{ml.openinghour}}</i-button></p>
          <p class="drawbutton"><i-button @click="configSpecialDays" type="primary">{{ml.openingspecialday}}configSpecialDays</i-button></p>
          <i-button type="primary" @click="publishNewMenu">{{ml.publish}}</i-button>
        </drawer>

        <modal
            ref="dialog"
            v-model="modalPublishNewMenu"
            :ok-text="ml.ok"
            :cancel-text="ml.cancel"
            :closable="false"
            :mask-closable="false"
            @on-ok="confirmPublishNewMenu"
            @on-cancel="cancelPublishNewMenu">
              <p style="color:#f60;text-align:center">
              <icon type="md-information-circle"></icon>
              <span>{{ml.askconfirmpublishnewmenu}}</span>
            </p>
          </modal>
        <businessInfo ref="businessInfo" :ml="ml" :data="data" :seller="seller" @closed="childDrawClosed"></businessInfo>
        <menuGroups ref="menuGroups" :ml="ml" :data="data" @closed="childDrawClosed"></menuGroups>
        <menus ref="menus" :ml="ml" :data="data" @closed="childDrawClosed"></menus>
        <openinghour ref="myOpeninghour" :ml="ml" :data="data" @closed="childDrawClosed"></openinghour>
        <specialdays ref="mySpecialDays" :ml="ml" :data="data" @closed="childDrawClosed"></specialdays>
    </div>
</template>
<script>
import businessInfo from 'components/config/businessInfo/businessInfo'
import menuGroups from 'components/config/menuGroups/menuGroups'
import menus from 'components/config/menus/menus'
import openinghour from 'components/config/openinghour/openinghour'
import specialdays from 'components/config/specialdays/specialdays'

export default {
  components: {
    businessInfo,
    menuGroups,
    menus,
    openinghour,
    specialdays
  },
  props: {
    ml: {},
    data: {},
    seller: {}
  },
  data () {
    return {
      styles: {
        height: 'calc(100% - 55px)',
        overflow: 'auto',
        paddingBottom: '53px',
        position: 'static'
      },
      configVisible: false,
      modalPublishNewMenu: false
    }
  },
  created() {
    // this.$root.eventHub.$on('signalr.onDownLoadMenuGroups', this.onDownLoadMenuGroups)
    // this.$root.eventHub.$on('signalr.onDeleteMenuGroup', this.onDeleteMenuGroup)
    // this.$root.eventHub.$on('signalr.onAddMenuGroup', this.onAddMenuGroup)
    this.$root.eventHub.$on('signalr.onPublishMenu', this.menuPublished)
  },
  mounted() {

  },
  methods: {
    showDraw() {
      this.configVisible = true
      console.log(data.options.isAdmin)
    },
    configSeller() {
      // alert('config selelr visiable')
      // this.sellerVisible = true
      this.configVisible = false
      this.$refs.businessInfo.showDraw()
    },
    configMenuGroups() {
      this.configVisible = false
      this.$refs.menuGroups.showDraw()
    },
    configMenus() {
      this.configVisible = false
      this.$refs.menus.showDraw()
    },
    configRiceTable() {

    },
    configOpeninghour() {
      this.configVisible = false
      this.$refs.myOpeninghour.showDraw()
    },
    configSpecialDays() {
      this.configVisible = false
      // alert(this.$refs.mySpecialDays)
      this.$refs.mySpecialDays.showDraw()
    },
    publishNewMenu() {
      this.modalPublishNewMenu = true
    },
    childDrawClosed() {
      console.log('childDrawClosed')
      this.configVisible = true
    },
    menuPublished() {
      this.$Modal.success({
        title: this.ml.success,
        content: this.ml.menupublished,
        okText: this.ml.ok
      });
    },
    confirmPublishNewMenu() {
      this.modalPublishNewMenu = false
      this.$root.eventHub.$emit('signalr.publishMenu')
    },
    cancelPublishNewMenu() {
      this.modalPublishNewMenu = false
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
