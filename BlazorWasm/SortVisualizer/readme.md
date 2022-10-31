# SortVisualizer

ufcpp.net の[ソートのページ](https://ufcpp.net/study/algorithm/sort.html)のデモ用の、ソートの可視化アプリ。

[Blazor Wasm](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/hosting-models?view=aspnetcore-6.0) が[来たら本気出すシリーズ](https://github.com/ufcpp/UfcppSample/labels/%E3%81%9D%E3%81%AE%E6%99%82%E3%81%8C%E6%9D%A5%E3%81%9F%E3%82%89%E6%9C%AC%E6%B0%97%E5%87%BA%E3%81%99)。

## Blazor 描画

1～Length までのデータを作ってソートして、
その過程を以下のような razor で描画してるだけ。

```razor
<div class="bars">
@{
    var w = _width / state.Items.Length;
    var wpx = $"{w}px";
}
@for (int i = 0; i < state.Items.Length; i++)
{
    var h = (int)Math.Floor(w * state.Items[i]);
    var hpx = $"{h}px";
    <span style="width: @wpx; height:@hpx;"></span>
}
</div>
```

## ソート アルゴリズムの追加

1. まず普通にソートするコードを書きます

```cs
public static void BubbleSort(int[] a)
{
    for (int i = 0; i < a.Length; i++)
        for (int j = 1; j < a.Length - i; j++)
        {
            if (a[j] < a[j - 1])
            {
                Swap(a, j, j - 1);
            }
        }
}
```

2. 比較のところと Swap のところに yield return を入れて、イテレーター化します。

```cs
public static IEnumerable<Operation> BubbleSort(int[] a)
{
    for (int i = 0; i < a.Length; i++)
        for (int j = 1; j < a.Length - i; j++)
        {
            yield return new(Kind.Compare, j, j - 1);
            if (a[j] < a[j - 1])
            {
                Swap(a, j, j - 1);
                yield return new(Kind.Swap, j, j - 1);
            }
        }
}
```

## 実物

オプション指定なし:

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/" width="780" height="500"></iframe></div>

ボタンとかいろいろ消すオプション:

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?i=0&s=0&w=150" width="780" height="500"></iframe></div>

1種類だけ表示とか:

<div><iframe src="https://black-ocean-009cb0000.2.azurestaticapps.net/?a=quick&i=0&s=0&w=300" width="304" height="332"></iframe></div>
