 <i-form :model="formMenu" label-position="top" ref="formMenu" :rules="ruleValidate" >
       
        <form-item :label="ml.rid">
                <i-input v-model="formMenu.rid"></i-input>
        </form-item>
       
        <form-item :label="ml.menunr">
                <i-input v-model="formMenu.menuNr"></i-input>
        </form-item>
       
        <form-item :label="ml.menuprice">
                <i-input v-model="formMenu.menuPrice"></i-input>
        </form-item>
       
        <form-item :label="ml.menutakeawayprice">
                <i-input v-model="formMenu.menuTakeAwayPrice"></i-input>
        </form-item>
       
        <form-item :label="ml.menugrouprid">
                <i-input v-model="formMenu.menuGroupRid"></i-input>
        </form-item>
       
        <form-item :label="ml.menudisplayorder">
                <i-input v-model="formMenu.menuDisplayOrder"></i-input>
        </form-item>
       
        <form-item :label="ml.menupreparationtime">
                <i-input v-model="formMenu.menuPreparationTime"></i-input>
        </form-item>
       
        <form-item :label="ml.menudisplayinweb">
                <i-input v-model="formMenu.menuDisplayInWeb"></i-input>
        </form-item>
       
        <form-item :label="ml.name1">
                <i-input v-model="formMenu.name1"></i-input>
        </form-item>
       
        <form-item :label="ml.name2">
                <i-input v-model="formMenu.name2"></i-input>
        </form-item>
       
        <form-item :label="ml.menuimage">
                <i-input v-model="formMenu.menuImage"></i-input>
        </form-item>
       
        <form-item :label="ml.description">
                <i-input v-model="formMenu.description"></i-input>
        </form-item>
       
        <form-item :label="ml.name3">
                <i-input v-model="formMenu.name3"></i-input>
        </form-item>
       
        <form-item :label="ml.name4">
                <i-input v-model="formMenu.name4"></i-input>
        </form-item>
              
 </i-form>

 formMenu: {

rid: '',

menuNr: '',

menuPrice: '',

menuTakeAwayPrice: '',

menuGroupRid: '',

menuDisplayOrder: '',

menuPreparationTime: '',

menuDisplayInWeb: '',

name1: '',

name2: '',

menuImage: '',

description: '',

name3: '',

name4: '',

},

 
      public string rid{get;set;}
       
      public string menuNr{get;set;}
       
      public string menuPrice{get;set;}
       
      public string menuTakeAwayPrice{get;set;}
       
      public string menuGroupRid{get;set;}
       
      public string menuDisplayOrder{get;set;}
       
      public string menuPreparationTime{get;set;}
       
      public string menuDisplayInWeb{get;set;}
       
      public string name1{get;set;}
       
      public string name2{get;set;}
       
      public string menuImage{get;set;}
       
      public string description{get;set;}
       
      public string name3{get;set;}
       
      public string name4{get;set;}
              

 ruleValidate: {

          rid: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuNr: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuPrice: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuTakeAwayPrice: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuGroupRid: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuDisplayOrder: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuPreparationTime: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuDisplayInWeb: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name1: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name2: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          menuImage: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          description: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name3: [
              { required: true, message: '*', trigger: 'blur' }
          ]
  
          name4: [
              { required: true, message: '*', trigger: 'blur' }
          ]
      
        },

public void Merge(Menu entity)
        {

            this.rid = entity.Rid;     
  
            this.menuNr = entity.MenuNr;     
  
            this.menuPrice = entity.MenuPrice;     
  
            this.menuTakeAwayPrice = entity.MenuTakeAwayPrice;     
  
            this.menuGroupRid = entity.MenuGroupRid;     
  
            this.menuDisplayOrder = entity.MenuDisplayOrder;     
  
            this.menuPreparationTime = entity.MenuPreparationTime;     
  
            this.menuDisplayInWeb = entity.MenuDisplayInWeb;     
  
            this.name1 = entity.Name1;     
  
            this.name2 = entity.Name2;     
  
            this.menuImage = entity.MenuImage;     
  
            this.description = entity.Description;     
  
            this.name3 = entity.Name3;     
  
            this.name4 = entity.Name4;     
             
        }

  public void MapToEntity(Menu entity)
        {

            entity.Rid = rid;
  
            entity.MenuNr = menuNr;
  
            entity.MenuPrice = menuPrice;
  
            entity.MenuTakeAwayPrice = menuTakeAwayPrice;
  
            entity.MenuGroupRid = menuGroupRid;
  
            entity.MenuDisplayOrder = menuDisplayOrder;
  
            entity.MenuPreparationTime = menuPreparationTime;
  
            entity.MenuDisplayInWeb = menuDisplayInWeb;
  
            entity.Name1 = name1;
  
            entity.Name2 = name2;
  
            entity.MenuImage = menuImage;
  
            entity.Description = description;
  
            entity.Name3 = name3;
  
            entity.Name4 = name4;
       
        }