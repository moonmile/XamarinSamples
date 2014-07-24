namespace FSharpNavigationPushAsync
open System
open System.Collections.Generic
open System.Linq
open System.Text
open Xamarin.Forms
open System.Threading.Tasks

module AsyncExtentions =
    type Async with 
        static member AwaitTaskVoid : (Task -> Async<unit>) =
                        Async.AwaitIAsyncResult >> Async.Ignore
open AsyncExtentions
    


type SubPage() as this =
    inherit ContentPage()
    do 
        this.Content <-
            new Label( 
                Text="This is SubPage",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand )

/// C#のAppクラスをそのまま直したもの
/// 1.ビルド時に --standalone をつけないと、Windows Phone プロジェクトで FSharp.Core のエラーが出る。
type App() =
(*
    static member GetMainPage() =
        new ContentPage(
            Content = new Label(
                Text = "Hello, Forms !",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand ))
*)

(*
    let pushNextPage(next) =
            let AwaitTaskVoid : (Task -> Async<unit>) =
                Async.AwaitIAsyncResult >> Async.Ignore
            page.Navigation.PushAsync(next) |> AwaitTaskVoid |> ignore
*) 
    static member GetMainPage() =
        let button = new Button( Text = "Go Next Page")
        let layout = new StackLayout()
        layout.Children.Add( new Label( Text = "Navigation.PushAsync"))
        layout.Children.Add( button )
        let page = new ContentPage ( Content = layout )
        button.Clicked.Add( fun(_) ->

                // C# の場合は
                // await page.Navigation.PushAsync(new SubPage()) 
                // これで動作する

                /// 1.これで良さそうな気もするのだがこれは動かない
                // let t = button.Navigation.PushAsync( page ) 
                // t.Start()
                // t.Wait()
                ///

                /// 2.スレッドを変えてみたが、Android/iOS だけ動く
                // 何故か Windows Phone では動かず
                // let tk = new Task(fun() -> 
                //    let t = page.Navigation.PushAsync(new SubPage()) 
                //    t.Start()
                // )
                // tk.RunSynchronously()

                /// 3. async すると戻り値がvoidなので、ややこしいのだが、
                /// この方式だと3プラットフォームで動く
                /// 参考 http://stackoverflow.com/questions/8022909/how-to-async-awaittask-on-plain-task-not-taskt
                /// async {} の場合、戻り値を持たない Task が使えないのでこうやるらしい。
                /// Page.Navigation.PushAsync が戻り値を持たないのでややこしいだけ。
                let AwaitTaskVoid : (Task -> Async<unit>) =
                     Async.AwaitIAsyncResult >> Async.Ignore
                page.Navigation.PushAsync(new SubPage()) |> AwaitTaskVoid |> ignore

                // 4. これは動かない。不思議だ。
                // async {
                //     do! page.Navigation.PushAsync(new SubPage()) |> Async.AwaitTaskVoid 
                // } |> ignore

            )
        new NavigationPage(page)
