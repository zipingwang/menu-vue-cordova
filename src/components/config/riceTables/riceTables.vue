<template>
    <div>
        <Drawer
            :title="ml.ricetable"
            v-model="show"
             :closable = "false"
            width="100%"
            :mask-closable="false"
            :styles="styles"
        >
          <div class="row">
              <div class="column" v-for="riceTable in riceTables">
                <div class="ricetableblock" @click="openRiceTable(riceTable)">
                      <div class="title">
                          {{riceTable.title}}
                      </div>
                      <div class="title">
                          {{riceTable.displayOrder}}
                      </div>

                      <div class="endfilling"></div>
                  </div>
              </div>
            </div> <!--row -->
            <div class="demo-drawer-footer">
              <Button  type="primary" style="margin-right: 8px" @click="close">{{ml.goback}}</Button>
              <Button type="primary" @click="addNewRiceTable">{{ml.add}}</Button>
          </div>
        </Drawer>
        <riceTable ref="riceTable" :ml="ml" :data="data"></riceTable>
    </div>
</template>
<script>
import riceTable from 'components/config/riceTable/riceTable'

export default {
  components: {
    riceTable
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
      riceTables: [],
      selectedRiceTable: {}
    }
  },
  created() {
    this.$root.eventHub.$on('signalr.onDownLoadRiceTables', this.onDownLoadRiceTables)
    this.$root.eventHub.$on('signalr.onDeleteRiceTable', this.onDeleteRiceTable)
    this.$root.eventHub.$on('signalr.onAddRiceTable', this.onAddRiceTable)
  },
  mounted() {

  },
  methods: {
    showDraw() {
      this.show = true
      this.$root.eventHub.$emit('signalr.sendMessageFromWebToServer', {'messageType': 'downLoadRiceTables'})
    },
    onDownLoadRiceTables(messageBody) {
      console.log('onDownLoadRiceTables in riceTables')
      // alert('onDownloadBusinessInfo')
      console.log(messageBody)
      console.log(typeof messageBody)
      this.riceTables = messageBody
      this.$Message.success(this.ml.success);
    },
    onDeleteRiceTable(messageBody) {
      console.log('onDeleteRiceTable in ricetables')
      console.log(messageBody)
      for (var index = 0; index < this.riceTables.length; index++) {
        if (this.riceTables[index].rid === messageBody.rid) {
          this.riceTables.splice(index, 1)
          console.log('ricetable deleted')
        }
      }
    },
    openRiceTable(riceTable) {
      console.log('open ricetable')
      this.selectedRiceTable = riceTable
      console.log(this.selectedRiceTable)
      this.$refs.riceTable.showDraw(riceTable)
    },
    addNewRiceTable() {
      var riceTable = {}
      riceTable.web = true
      riceTable.displayOrder = 10
      this.$refs.riceTable.showDraw(riceTable)
    },
    onAddRiceTable(newRiceTable) {
      console.log('onAddRiceTable')
      this.riceTables.push(newRiceTable)
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
