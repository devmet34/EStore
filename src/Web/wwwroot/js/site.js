//todo closure for protecting js from global
const estore_site = function () {
  var t = 0;
  const divProducts = document.querySelector("#divProducts");
  function log(msg) {

    console.log(new Date() + " " + msg);

  }

  function changeProductsHtml(resp) {
    //divProducts.innerHTML = resp;
    //divProducts.replaceChildren(resp);
    $("#divProductCards").replaceWith(resp);

  }

  const getForgeryToken = () => {
    try {
      const forgeryToken = document.querySelector('[name=__RequestVerificationToken]').value;
      return forgeryToken
    }
    catch (ex) {
      throw new Error("problem getting forgery token." + ex);
    }
  }


  //MC vanilla js binding to form submit event
  const findForm = document.querySelector("#findForm");
  if (findForm) {
    document.querySelector("#findForm").addEventListener("submit", function (e) {
      e.preventDefault();
      findProducts();
    });
  }

  function findProducts() {
    const uri = "findproducts";
    const find = document.querySelector("#find").value;
    $.ajax(uri, {
      data: { productName: find }
    }).done(resp => {
      changeProductsHtml(resp);
    });
  }

  $("#sortby").on("change", function (e) {
    e.preventDefault();
    sortProductsBy();

  });
  function sortProductsBy() {
    const uri = "sortproducts";
    const sort = $('#sortby').val();

    //sessionStorage.setItem("sortby", sort);
    $.ajax(uri,
      {
        data: { "sortBy": sort }
      }
    ).done((resp) => {
      changeProductsHtml(resp)
      //document.getElementById("products").innerHTML = res;
    });

    //document.getElementById("filterForm").submit();
  }

  $("#formFilterProds").on("submit", function (e) {
    e.preventDefault();
    let formData = new FormData(this);
    let formObj = Object.fromEntries(formData);
    let formSeri = $(this).serialize();
    const uri = "filterproducts";
    const formJson = JSON.stringify(formObj);
    let formArray = $(this).serializeArray();
    let mainCatHidden = this.querySelector("#mainCatHidden");
    this.querySelectorAll('#mainCat').forEach(function (inp) {
      if (inp.checked === true) {
        formObj.mainCat = formObj.subCat;
        formObj.subCat = null;
      }
    });

    $.ajax(uri, {
      method: "get",
      data: formObj

    }).done(r => {
      changeProductsHtml(r);
      productFilterFormObj = formObj;
    })
      .fail(r => {
        log("failed " + r.status)
      });

    var g = 1;
    //return false;

  });


  var productFilterFormObj = {};

  $("#btnFilterGet").on("click", function (e) {
    let catsContainer = document.querySelector("#catsContainer");
    if (catsContainer.children.length > 0) { return; }
    const uri = "filter/GetCats";
    const getFilters = $.ajax(uri, {
      method: "get"

    }).done(r => {
      log("getfilters done")
      loadFilterCats(r);
    }).fail(r => log("getfilters failed"));
    /*
    getFilters.done(log("getfilters done"));
     getFilters.fail(log("getfilters failed"));
    */


  });
 
  function loadFilterCats(obj) {
    let catsContainer = document.querySelector("#catsContainer");
    //catsContainer.insertAdjacentElement("beforeend", `<input type='text' id='mainCatHidden' name='mainCat' hidden />`);
    for (var key in obj) {
      catsContainer.insertAdjacentHTML("beforeend", `<h6 class='text-decoration-underline'>${key}</h6>`);
      
      catsContainer.insertAdjacentHTML("beforeend", ` &nbsp &nbsp   
     ${key}  <input type='radio' id='mainCat' name='subCat' value='${key}' /> <br/>`);
      
      for (var index in obj[key]) {
        //<label>${obj[key][val]}</label>
        const val = obj[key][index];
        catsContainer.insertAdjacentHTML("beforeend", `&nbsp &nbsp   
     ${val} <input type='radio' name='subCat' value='${val}' /> <br/>`);
      }
    }

  }

  function resetFilterForm(e) {
    const resetBtn = document.querySelector("#btnResetFilter");
    
  }


  /* bind to bs modal load event
  $('#modalFilter').on('shown.bs.modal',function() {
    let catsContainer = document.querySelector("#catsContainer");
    catsContainer.innerHTML +='<h5>asdasdasdsd</h5>'
    var a = 1;
  });
  */

 //mc using event delegation bubbling 
  $("#divProducts").on("click","#nextPage", function (e) {
    //e.preventDefault(); //mc this prevents auto scrolling of page to top after clicking next btn.
    goNextPage();
  });
  function goNextPage() {
    const pageNumber = parseInt($('#pageN').text()) + 1;
    goToPage(pageNumber);
  }

  $("#divProducts").on("click","#prevPage", function (e) {
    //e.preventDefault();
    goPrevPage();
  });

  function goPrevPage() {
    const page = parseInt($('#pageN').text()) - 1
    goToPage(page)
  }
  function goToPage(pageNumber) {
    const sort = $('#sortby').val();
    const find = $('#find').val();

    //const page = pageNumber //parseInt( $('#pageN').val())+ 1;
    const uri = "getproductsbypage";


    $.ajax(uri,
      {
        method: "get",
        data: {
          page: pageNumber, sortBy: sort, find: find, mainCat: productFilterFormObj['mainCat'], subCat: productFilterFormObj['subCat'], priceMin: productFilterFormObj['priceMin'], priceMax: productFilterFormObj['priceMax']
        }

      }
    ).done(resp => {
      changeProductsHtml(resp);
      changePageN(pageNumber);

    })


  }

  function changePageN(pageNumber) {
    let pageN = document.getElementById("pageN");
    pageN.textContent = pageNumber;
    if (pageNumber > 1)
      ActivatePrev();


  }

  function testJquery() {
    $('#button1').val = "bej";
  }

  function ActivatePrev() {

    const prev = document.getElementById("prevPage");
    prev.hidden = false;

  }

  /*
  $("#basketBtn").on("click", function () {
    getBasket();
  });
  */
  function getBasket() {

    const uri = "basket/getbasket"
    fetch(uri, {})
      .then(res => res.text())
      .then(r => {
        $('#basket').html(r);
        $('#basket').show();

      }

      );

  }

  function closeBasket() {
    $('#basket').hide();
  }

  function updateBasketCount() {
    const uri = "basket/getbasketcount";
    //$('#basketToggle').text(' ' + basketCount)

    fetch(uri, {
      method: "get"

    }).then(r => r.text()).then(r =>
      $('#basketBtn').text(' ' + r)
    );


  }

  function setBasketItem(productId, isFromBasket) {


    const qt = !isFromBasket ? document.getElementById(productId + "-qt").value
      : document.getElementById(productId).value;
    const forgeryToken = getForgeryToken();
    const baseUrl = document.URL;
    const uri = "basket/setbasketitem"
    //const url = baseUrl + "addproduct";
    console.debug("test:{id}" + productId);

    $.ajax(uri, {

      method: "post",
      headers: { RequestVerificationToken: forgeryToken },
      data: { productId: productId, qt: qt }

    }
    ).done(() => {
      updateBasketCount();
      if (isFromBasket)
        getBasket();
      showToast("Item added to basket",false);

    }).fail(() => {
      showToastFail()
    });



    //post(url, id);

  }


  async function removeBasketItem(productId) {
    const uri = "basket/removebasketitem";
    const forgeryToken = getForgeryToken();

    const remove =
      await fetch(uri,
        {
          method: "post",
          headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": forgeryToken
          },

          body: JSON.stringify(productId)

        }
      );

    let resp = await remove;
    if (!resp.ok) {
      log("Error during removing product");
      //showToastFail();
      return;
    }
    getBasket();
    decreaseBasketCount();
    //showToastSuccess();

  }

  const toastFailHtml = `
  <div class="toast-container fixed-top d-flex alert alert-danger justify-content-center align-items-center" role="alert" aria-live="assertive" aria-atomic="true">
    <div id="bsToast" class="toast" role="alert" data-bs-delay="2000" data-bs-autohide="true">
      <div class="toast-body">
        FAILED
      </div>
      <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
   </div>
  </div>
  `

  const toastSuccessHtml = `
  <div  class="toast-container fixed-top d-flex alert alert-success justify-content-center align-items-center" role="alert" aria-live="assertive" aria-atomic="true">
    <div id="bsToast" class="toast" role="alert" data-bs-delay="2000" data-bs-autohide="true">
      <div class="toast-body">
        SUCCESS
      </div>
      <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
   </div>
  </div>
  `

  function showToastFail() {
    showToast("FAIL", true);
  }

  function showToastSuccess() {
    showToast("SUCCESS", false);
  }

  /**     //mc jsdoc
  @param {string} str
  @param {bool} alert
  */
  function showToast(str, alert) {
    if (str == null) {
      log("No string given for toast");
      return;
    }
    const toast = document.querySelector("#bsToastCustom");
    toast.querySelector("#toastBody").textContent = str;

    if (alert === false)
      toast.className = "toast alert-success";
      

      


    /*
    if (n === 0) {
      toast = document.getElementById("bsToastFail")

    }
    else {
      toast = document.getElementById("bsToastSuccess")
    }
    */
    
    let bsToast = bootstrap.Toast.getOrCreateInstance(toast);
    bsToast.show();
  }
  /*
function showToast(n) {
  
  if (n == 0) {

    document.body.innerHTML += toastFailHtml;
  }
  else { document.body.innerHTML +=toastSuccessHtml }
  //setTimeout(null, 1000);
  const bsToastEl = document.getElementById("bsToast");
  const toastEl =   bootstrap.Toast.getOrCreateInstance(bsToastEl);
  toastEl.show();
}
*/


  function decreaseBasketCount() {
    let basketToggle = $('#basketToggle');
    let basketCount = basketToggle.text();
    const newCount = parseInt(basketCount) - 1;
    basketToggle.text(' ' + newCount);
    //$('#basketToggle').text(' ' + basketCount)
  }

  function refreshBasket(html) {
    $('#basket').html(html);
  }

  function getOrderDetails(el) {
    el.nextElementSibling.hidden = !el.nextElementSibling.hidden
  }

  function test() {
    const forgeryToken = getForgeryToken();
    const id = 3;
    const baseUrl = document.URL;
    const uri = "addproduct"
    const url = baseUrl + "addproduct";
    console.debug("test:{id}" + id);


  }

  /*
  function increaseBasket() {
    let basketToggle = $('#basketToggle');
    let basketCount = basketToggle.text();
    const newCount = parseInt(basketCount)+1;
    basketToggle.text(' '+newCount);
    //$('#basketToggle').text(' ' + basketCount)
  }
  
  function decreaseBasket() {
    let basketToggle = $('#basketToggle');
    let basketCount = basketToggle.text();
    const newCount = parseInt(basketCount) - 1;
    basketToggle.text(' ' + newCount);
    //$('#basketToggle').text(' ' + basketCount)
  }
  
  function SubtractProduct(productId) {
  
    const forgeryToken = getForgeryToken();
    const baseUrl = document.URL;
    const uri = "addproduct"
    //const url = baseUrl + "addproduct";
    console.debug("test:{id}" + productId);
  
    $.ajax(uri, {
  
      method: "post",
      headers: { RequestVerificationToken: forgeryToken },
      data: { productId: productId }
  
    }
    );
  
    //post(url, id);
  
  }
  
  */

  /*
  convert form to json
  const formObj = Object.fromEntries(new FormData(e));
  const json = JSON.stringify(formObj);
  
  
  */

  return {
    getBasket: getBasket, closeBasket: closeBasket, setBasketItem: setBasketItem, removeBasketItem: removeBasketItem, setBasketItem: setBasketItem, resetFilterForm: resetFilterForm, showToast:showToast, getOrderDetails:getOrderDetails
  }
}();

//estore_site();